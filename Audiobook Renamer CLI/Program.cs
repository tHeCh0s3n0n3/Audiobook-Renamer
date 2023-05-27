using Audiobook_Renamer_CLI;
using CommandLine;
using MetadataHelper;
using Serilog;
using ShellProgressBar;
using System.Collections.Concurrent;
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
    private static int currentCopyCount = 0;
    private static int totalCopyCount = 0;

    private static readonly ConcurrentBag<BookToCopy> _filesToCopy = new();

    private static readonly SemaphoreSlim _semaphoreSlim;

    static Program()
    {
        _semaphoreSlim = new SemaphoreSlim(3);
    }

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
        _ = Interlocked.Exchange(ref currentCopyCount, 0);

        try
        {
            if (!_cancellationTokenSource.TryReset())
            {
                throw new InvalidOperationException("Cannot reset cancellation token source");
            }

            ProgressBar progressBar = new(100, "Copying Books");

            CancellationToken cancellationToken = _cancellationTokenSource.Token;
            await Task.Run(() =>
                Parallel.ForEach(_audiobooks
                                 , _parallelOptions
                                 , item => DetermineFilesToCopy(Arguments.DestinationDir, item, progressBar, cancellationToken))
                , cancellationToken);

            if (currentCount % 10 != 0)
            {
                Log.Verbose("Processed {0}/{1}", currentCount, totalCount);
            }

            //await Task.Run(() =>
            //    Parallel.ForEach(_filesToCopy
            //                     , _parallelOptions
            //                     , item => Audiobook.CopyBookWithProgress(item, cancellationToken))
            //    , cancellationToken);

            _ = Interlocked.Exchange(ref totalCopyCount, _filesToCopy.Count);

            List<Task> tasks = new();
            foreach (var item in _filesToCopy)
            {
                tasks.Add(CopyBookWithProgress(item.SourceFile, item.DestinationFile, item.Progress, progressBar, cancellationToken));
            }

            await Task.WhenAll(tasks);

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

    private static void DetermineFilesToCopy(string basePath, Audiobook item, ProgressBar ppbar, CancellationToken cancellationToken)
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
                string finalPath = item.CreateDirectoryStructure(basePath);
                if (File.Exists(finalPath))
                {
                    return;
                }

                FileInfo sourceFile = new(item.Filename);
                ChildProgressBar cpbar = ppbar.Spawn(100, sourceFile.Name);

                BookToCopy newFileToCopy = new(new(item.Filename), new(finalPath), cpbar);
                _filesToCopy.Add(newFileToCopy);
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

    public static async Task CopyBookWithProgress(FileInfo sourceFile
                                                  , FileInfo destinationFile
                                                  , ChildProgressBar thisProgress
                                                  , ProgressBar parentProgress
                                                  , CancellationToken cancellationToken = default)
    {
        try
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            const int bufferSize = 1024 * 1024;
            byte[] buffer = new byte[bufferSize];

            using FileStream source = sourceFile.OpenRead();
            using FileStream dest = destinationFile.OpenWrite();

            long totalSize = sourceFile.Length;

            dest.SetLength(source.Length);
            int readBytes;
            for (long size = 0; size < totalSize; size += readBytes)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    source.Close();
                    dest.Close();
                    destinationFile.Delete();
                    return;
                }

                thisProgress.Tick(Convert.ToInt32(size * 100 / totalSize));
                //Thread.Sleep(500);
                readBytes = source.Read(buffer, 0, bufferSize);
                await dest.WriteAsync(buffer.AsMemory(0, readBytes), cancellationToken);
            }
            source.Close();
            await dest.FlushAsync(cancellationToken);
            dest.Close();

            thisProgress.Tick(Convert.ToInt32(totalSize * 100 / totalSize));
        }
        finally
        {
            Interlocked.Add(ref currentCopyCount, 1);
            parentProgress.Tick(currentCopyCount * 100 / totalCopyCount);
            _semaphoreSlim.Release();
        }
    }
}