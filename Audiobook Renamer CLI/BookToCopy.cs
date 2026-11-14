using ShellProgressBar;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace AudiobookRenamerCLI;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public class BookToCopy(FileInfo sourceFilename
                        , FileInfo destinationFilename
                        , ChildProgressBar progress)
{

    public FileInfo SourceFile { get; set; } = sourceFilename
                                               ?? throw new ArgumentNullException(nameof(sourceFilename));

    public FileInfo DestinationFile { get; set; } = destinationFilename
                                                    ?? throw new ArgumentNullException(nameof(destinationFilename));

    public ChildProgressBar Progress { get; set; } = progress;

    public bool IsCopied { get; set; } = false;
}
