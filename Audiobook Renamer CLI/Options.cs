using CommandLine;
using CommandLine.Text;
using System.Text;

namespace Audiobook_Renamer_CLI;

public class Options
{
    private readonly bool _verbose;
    private readonly bool _quiet;
    private readonly string _sourceDir;
    private readonly string _destinationDir;
    private readonly int _parallelCount;

    [Option('v', "verbose", Default = false, Required = false, HelpText = "See more output.", SetName = "Verbose")]
    public bool Verbose
        => _verbose;

    [Option('q', "quiet", Default = false, Required = false, HelpText = "Do not show any output.", SetName = "Quiet")]
    public bool Quiet
        => _quiet;

    [Value(0, MetaName = "Source Directory", Required = true, HelpText = "Directory containing audiobooks to rename.")]
    public string SourceDir
        => _sourceDir;

    [Value(1, MetaName = "Destination Directory", Required = true, HelpText = "Directory to place the renamed files.")]
    public string DestinationDir
        => _destinationDir;

    [Value(5, MetaName = "Parallel Files", Required = false, HelpText = "Number of files to copy in parallel.")]
    public int ParallelCount
        => _parallelCount;

    public Options(bool verbose, bool quiet, string sourceDir, string destinationDir, int parallelCount)
    {
        _verbose = verbose;
        _quiet = quiet;
        _sourceDir = sourceDir;
        _destinationDir = destinationDir;
        _parallelCount = parallelCount;
    }

    [Usage]
    public static IEnumerable<Example> Examples
        => new List<Example>()
        {
            new Example("Show max output", new Options(true, false, @"C:\InDir\", @"C:\OutDir\", 5))
            , new Example("Quiet as a mouse", new Options(false,true, @"C:\InDir\", @"C:\OutDir\", 5))
        };

    /// <inheritdoc cref="object.ToString"/>
    /// <inheritdoc cref="StringBuilder.AppendLine()" path="/exception"/>
    /// <inheritdoc cref="StringBuilder.Append" path="/exception"/>
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine("Options:");
        sb.Append("    Quiet: ").AppendLine(Quiet ? "true" : "false");
        sb.Append("  Verbose: ").AppendLine(Verbose ? "true" : "false");
        sb.Append("       Source Directory: ").AppendLine(SourceDir);
        sb.Append("  Destination Directory: ").AppendLine(DestinationDir);
        sb.Append("          Parallel Copy: ").AppendLine(ParallelCount.ToString());

        return sb.ToString();
    }
}
