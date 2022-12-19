using System.Text.RegularExpressions;

namespace MetadataHelper;

public sealed partial class Audiobook
{
    public string Filename { get; set; }

    public string Title { get; set; }

    public string SafeTitle
        => StripIllegalChars(Title);

    public string Author { get; set; }

    public string SafeAuthor
        => StripIllegalChars(Author);

    public string? Series { get; set; }

    public string SafeSeries
        => StripIllegalChars(Series);

    public string? BookNumber { get; set; }

    public string? JSONRepresentation { get; set; }

    public Audiobook(string filename)
        : this(filename, null, string.Empty, string.Empty, null, null)
    { }

    public Audiobook(string filename, string? json, string title, string author)
        : this(filename, json, title, author, null, null)
    { }

    public Audiobook(string filename, string? json, string title, string author, string series)
        : this(filename, json, title, author, series, null)
    { }

    public Audiobook(string filename, string? json, string title, string author, string? series, string? bookNumber)
    {
        Filename = filename;
        JSONRepresentation = json;
        Title = title;
        Author = author;
        Series = series;
        BookNumber = bookNumber;
    }

    /// <summary>
    /// Strips illegal filename characters from a string
    /// </summary>
    /// <param name="input">String to strip illegal characters from</param>
    /// <returns>String without the illegal characters</returns>
    /// <inheritdoc cref="Regex.Replace" path="/exception"/>
    private static string StripIllegalChars(string? input)
    {
        if (input is null)
        {
            return string.Empty;
        }

        return IllegalCharactersRegex().Replace(input
, string.Empty);
    }

    /// <summary>
    /// Creates a the directory structure and copes the book the new directory
    /// </summary>
    /// <param name="basePath">The base path where we want to create the book</param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="RegexMatchTimeoutException"></exception>
    /// <exception cref="IOException"></exception>
    /// <exception cref="UnauthorizedAccessException"></exception>
    /// <exception cref="PathTooLongException"></exception>
    /// <exception cref="DirectoryNotFoundException"></exception>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public void CreateDirectoryStructureAndCopyBook(string basePath)
    {
        string finalPath;
        if (!string.IsNullOrEmpty(SafeSeries)
            && !string.IsNullOrEmpty(BookNumber))
        {
            string strippedBookName = Regex.Replace(SafeTitle, SafeSeries, string.Empty).Trim();
            string bookDir;

            bookDir = !string.IsNullOrWhiteSpace(strippedBookName)
                      ? $"Book {BookNumber} - {strippedBookName}"
                      : $"Book {BookNumber} - {SafeTitle}"; // The title of the book is probably
                                                            // the name as the series keep the
                                                            // title as is.

            finalPath = Path.Combine(new[] { basePath, SafeAuthor, SafeSeries, bookDir });
        }
        else
        {
            finalPath = Path.Combine(new[] { basePath, Author, SafeTitle });
        }

        Directory.CreateDirectory(finalPath);
        string finalFilename = Path.Combine(finalPath, Path.GetFileName(Filename));
        if (!File.Exists(finalFilename))
        {
            File.Copy(Filename, finalFilename, false);
        }
    }

    [GeneratedRegex("[<,>,:,\",/,\\,|,?,*]")]
    private static partial Regex IllegalCharactersRegex();
}
