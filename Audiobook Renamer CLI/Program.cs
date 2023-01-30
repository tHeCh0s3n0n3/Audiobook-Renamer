using Audiobook_Renamer_CLI;
using CommandLine;
using MetadataHelper;
using Serilog;
using System.Reflection;

namespace AudiobookRenamerCLI;

public static class Program
{
    public const int RETVAL_SUCCESS = 0;
    public const int RETVAL_ERROR_GENERAL = 1;
    public const int RETVAL_ERROR_SOURCE_DIR_MISSING = 2;
    public const int RETVAL_ERROR_DEST_DIR_MISSING = 4;
    public const int RETVAL_ERROR_OPERATION_CANCELED = 8;

    private static readonly int _retval = RETVAL_SUCCESS;

    private static ParserResult<Options>? _parserResult;
    private static Options? Arguments =>
        _parserResult?.Value;

    private static List<Audiobook> _audiobooks = new();

    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private static readonly ParallelOptions _parallelOptions = new();

    private static Exception? _exception;

    private static int currentCount = 0;
    private static int totalCount = 0;

    public static void Prepare()
    {
        _parallelOptions.MaxDegreeOfParallelism = 5;
    }

    [STAThread]
    public static async Task<int> Main(string[] args)
    {
        Prepare();

        try
        {
            using Parser parser = new(p =>
            {
                p.CaseSensitive = false;
                p.HelpWriter = Console.Error;
            });
            _parserResult = parser.ParseArguments<Options>(args);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return _retval | RETVAL_ERROR_GENERAL;
        }

        if (Arguments is null)
        {
            return _retval | RETVAL_ERROR_GENERAL;
        }

        SetupLogger(Arguments.Verbose, Arguments.Quiet);

        var assembly = Assembly.GetExecutingAssembly().GetName();
        Log.Information($"{assembly.Name} - v{assembly.Version}");

        Log.Verbose("Options:");
        Log.Verbose("  Quiet: {0}", Arguments.Quiet);
        Log.Verbose("  Verbose: {0}", Arguments.Verbose);
        Log.Verbose("  Source Directory: {0}", Arguments.SourceDir);
        Log.Verbose("  Destination Directory: {0}", Arguments.DestinationDir);

        if (!Directory.Exists(Arguments.SourceDir))
        {
            Log.Fatal("Source directory doesn't exist.");
            return _retval | RETVAL_ERROR_SOURCE_DIR_MISSING;
        }

        if (!Directory.Exists(Arguments.DestinationDir))
        {
            try
            {
                Log.Warning("Destination directory missing, creating it.");
                Directory.CreateDirectory(Arguments.DestinationDir);
            }
            catch
            {
                Log.Fatal("Destination directory couldn't be created.");
                return _retval | RETVAL_ERROR_DEST_DIR_MISSING;
            }
        }

        Log.Verbose("Parsing audiobooks from source directory");
        _audiobooks = Helper.ParseDirectory(Arguments.SourceDir, Helper.FileTypes.MP3);
        Log.Information("Found {0} audiobooks in {1}", _audiobooks.Count, Arguments.SourceDir);

        _ = Interlocked.Exchange(ref currentCount, 0);
        _ = Interlocked.Exchange(ref totalCount, _audiobooks.Count);

        try
        {
            if (!_cancellationTokenSource.TryReset())
            {
                throw new InvalidOperationException("Cannot reset cancellation token source");
            }

            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            await Task.Run(() =>
                Parallel.ForEach(_audiobooks
                                 , _parallelOptions
                                 , item => DoWork(Arguments.DestinationDir, item, cancellationToken))
                , cancellationToken);

            if (currentCount % 10 != 0)
            {
                Log.Verbose("Processed {0}/{1}", currentCount, totalCount);
            }

            if (cancellationToken.IsCancellationRequested
                && _exception is not null)
            {
                throw _exception;
            }
        }
        catch (OperationCanceledException ocex)
        {
            Log.Fatal(ocex, "Operation Canceled: ");
            if (_exception is not null)
            {
                Log.Fatal(_exception, "Possibly due to: ");
            }
            return _retval | RETVAL_ERROR_OPERATION_CANCELED;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "General Exception: ");
            return _retval | RETVAL_ERROR_GENERAL;
        }

        Log.Information("Done");
        return _retval | RETVAL_SUCCESS;
    }

    private static void SetupLogger(bool verbose, bool quiet)
    {
        LoggerConfiguration logConfig = new();
        logConfig = (verbose
                     ? logConfig.MinimumLevel.Verbose()
                     : logConfig.MinimumLevel.Information());

        logConfig = logConfig.WriteTo
                             .File(@"Logs\.log"
                                   , rollingInterval: RollingInterval.Day);
        if (!quiet)
        {
            logConfig = logConfig.WriteTo.Console();
        }

        Log.Logger = logConfig.CreateLogger();
    }

    private static void DoWork(string basePath, Audiobook item, CancellationToken cancellationToken)
    {
        if (_cancellationTokenSource is null) return;

        Interlocked.Increment(ref currentCount);
        if (currentCount % 10 == 0)
        {
            Log.Verbose("Processed {0}/{1}", currentCount, totalCount);
        }

        try
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                item.CreateDirectoryStructureAndCopyBook(basePath);
            }
        }
        catch (Exception ex) when (
            ex is ArgumentException
            || ex is ArgumentNullException
            || ex is System.Text.RegularExpressions.RegexMatchTimeoutException
            || ex is IOException
            || ex is UnauthorizedAccessException
            || ex is PathTooLongException
            || ex is DirectoryNotFoundException
            || ex is FileNotFoundException
            || ex is NotSupportedException)
        {
            SetException(ex);
            _cancellationTokenSource.Cancel();
        }
    }

    public static void SetException(Exception ex)
    {
        _exception = ex;
    }
}