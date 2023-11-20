using ShellProgressBar;

namespace Audiobook_Renamer_CLI;

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
