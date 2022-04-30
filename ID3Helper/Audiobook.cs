using System.Text.RegularExpressions;

namespace ID3Helper;

public class Audiobook
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

    private static string StripIllegalChars(string? str)
    {
        if (str is null)
        {
            return string.Empty;
        }

        return Regex.Replace(str
                             , "[<,>,:,\",/,\\,|,?,*]"
                             , string.Empty);
    }

    public void CreateDirectoryStructureAndCopyBook(string basePath)
    {
        string finalPath;
        if (!string.IsNullOrEmpty(SafeSeries)
            && !string.IsNullOrEmpty(BookNumber))
        {
            string strippedBookName = Regex.Replace(SafeTitle, SafeSeries, string.Empty).Trim();
            string bookDir;
            if (!string.IsNullOrWhiteSpace(strippedBookName))
            {
                bookDir = $"Book {BookNumber} - {strippedBookName}";
            }
            else
            { // The title of the book is probably the name as the series, keep the title as is.
                bookDir = $"Book {BookNumber} - {SafeTitle}";
            }
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
}
