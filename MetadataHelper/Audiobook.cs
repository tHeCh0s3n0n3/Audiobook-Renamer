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

    public ICollection<AudiobookChapter>? Chapters { get; set; }

    public Audiobook(string filename)
        : this(filename, null, string.Empty, string.Empty, null, null, null)
    { }

    public Audiobook(string filename, string? json, string title, string author)
        : this(filename, json, title, author, null, null, null)
    { }

    public Audiobook(string filename, string? json, string title, string author, string series)
        : this(filename, json, title, author, series, null, null)
    { }

    public Audiobook(string filename, string? json, string title, string author, string? series, string? bookNumber, ICollection<AudiobookChapter>? chapters)
    {
        Filename = filename;
        JSONRepresentation = json;
        Title = title;
        Author = author;
        Series = series;
        BookNumber = bookNumber;
        Chapters = chapters;
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
    /// <inheritdoc cref="CreateDirectoryStructure(string)" path="/exception "/>
    /// <inheritdoc cref="File.Copy(string, string, bool)" path="/exception"/>
    public void CreateDirectoryStructureAndCopyBook(string basePath)
    {
        string finalFilename = CreateDirectoryStructure(basePath);
        if (!File.Exists(finalFilename))
        {
            File.Copy(Filename, finalFilename, false);
        }
    }

    /// <summary>
    /// Creates the required directory structure and returns the final file path
    /// </summary>
    /// <param name="basePath">The base path where we want to create the book</param>
    /// <returns>The full path where the book should be copied to</returns>
    /// <inheritdoc cref="Regex.Replace(string, string, string)" path="/exception"/>
    /// <inheritdoc cref="Path.Combine(string[])" path="/exception"/>
    /// <inheritdoc cref="Path.GetFileName(string?)" path="/exception"/>
    /// <inheritdoc cref="Directory.CreateDirectory(string)" path="/exception"/>
    public string CreateDirectoryStructure(string basePath)
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
        return Path.Combine(finalPath, Path.GetFileName(Filename));
    }

    [GeneratedRegex("[<,>,:,\",/,\\,|,?,*]")]
    private static partial Regex IllegalCharactersRegex();
}
