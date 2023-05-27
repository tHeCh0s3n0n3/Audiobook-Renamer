using ShellProgressBar;

namespace Audiobook_Renamer_CLI;

public class BookToCopy
{

    public FileInfo SourceFile { get; set; }

    public FileInfo DestinationFile { get; set; }

    public ChildProgressBar Progress { get; set; }

    public bool IsCopied { get; set; } = false;

    public BookToCopy(FileInfo sourceFilename, FileInfo destinationFilename, ChildProgressBar progress)
    {
        SourceFile = sourceFilename ?? throw new ArgumentNullException(nameof(sourceFilename));
        DestinationFile = destinationFilename ?? throw new ArgumentNullException(nameof(destinationFilename));
        Progress = progress;
    }
}
