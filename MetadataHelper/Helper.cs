using ATL;
using System.Text;

namespace MetadataHelper;

public class Helper
{
    public enum FileTypes
    {
        MP3 = 0x0,
        M4A = 0x1,
        M4B = 0x2
    }

    /// <summary>
    /// Parses the metadata tags of an audiobook and returns an <see cref="Audiobook"/> object if parsing
    /// was successful
    /// </summary>
    /// <param name="filename">Full file path</param>
    /// <returns><see cref="Audiobook"/> object if successful, <see langword="null"/> otherwise.</returns>
    public static Audiobook? ParseFile(string filename)
    {
        try
        {
            string? title = null;
            string? author = null;
            string? seriesName = null;
            string? bookNumber = null;
            string? utfJson = null;

            Track track = new(filename);
            title = track.Title;
            author = track.AlbumArtist;
            seriesName = string.IsNullOrEmpty(track.SeriesTitle)
                         ? track.AdditionalFields.TryGetValue("SERIES", out string? seriesTitle)
                           ? seriesTitle
                           : string.Empty
                         : track.SeriesTitle;
            bookNumber = string.IsNullOrEmpty(track.SeriesPart)
                         ? track.AdditionalFields.TryGetValue("SERIES-PART", out string? seriesPart)
                           ? seriesPart
                           : string.Empty
                         : track.SeriesPart;
            
            if (track.AdditionalFields.TryGetValue("json64", out string? json)
                && json is not null)
            {
                utfJson = Encoding.UTF8.GetString(Convert.FromBase64String(json));
            }

            if (string.IsNullOrEmpty(author))
            {
                return null;
            }

            AudiobookChapter[] chapters = new AudiobookChapter[track.Chapters.Count];

            int i = 0;
            foreach (var chapter in track.Chapters.OrderBy(c => c.UniqueID))
            {
                if (!chapter.UseOffset)
                {
                    chapters[i] = new(Title: chapter.Title
                                     , Start: TimeSpan.FromMilliseconds(chapter.StartTime)
                                     , End: TimeSpan.FromMilliseconds(chapter.EndTime)
                                     , Duration: TimeSpan.FromMilliseconds(chapter.EndTime - chapter.StartTime));
                }
                else
                {
                    chapters[i] = new(chapter.Title);
                }
                i++;
            }

            return new Audiobook(filename
                                 , utfJson
                                 , title
                                 , author
                                 , seriesName
                                 , bookNumber
                                 , chapters.Any(c => c is not null)
                                   ? chapters
                                   : null);
        }
        catch (Exception ex)
        when (ex is ArgumentException
              || ex is ArgumentNullException
              || ex is DecoderFallbackException
              || ex is FormatException)
        {
            return null;
        }
    }

    /// <summary>
    /// Get all files of filetype(s) from a directory and parse the ID3 tags of
    /// each one then return the resulting list.
    /// </summary>
    /// <param name="directory">Directory to parse</param>
    /// <returns>List of <see cref="Audiobook"/> objects parsed from the directory.</returns>
    /// <inheritdoc cref="System.IO.Directory.GetFiles" path="/exception"/>
    /// <inheritdoc cref="System.Linq.Enumerable.Select" path="/exception"/>
    /// <inheritdoc cref="System.Linq.Enumerable.ToList" path="/exception"/>
    public static List<Audiobook> ParseDirectory(string directory, FileTypes fileTypes)
    {
        List<Audiobook> retval = new();

        if (fileTypes.HasFlag(FileTypes.MP3))
        {
            List<Audiobook> mp3Files = GenericParseDirectory(directory, "*.mp3", ParseFile);

            if (mp3Files.Any())
            {
                retval.AddRange(mp3Files);
            }
        }

        if (fileTypes.HasFlag(FileTypes.M4A))
        {
            List<Audiobook> m4aFiles = GenericParseDirectory(directory, "*.m4a", ParseFile);
            
            if (m4aFiles.Any())
            {
                retval.AddRange(m4aFiles);
            }
        }

        if (fileTypes.HasFlag(FileTypes.M4B))
        {
            List<Audiobook> m4bFiles = GenericParseDirectory(directory, "*.m4b", ParseFile);

            if (m4bFiles.Any())
            {
                retval.AddRange(m4bFiles);
            }
        }

        return retval;
    }

    /// <summary>
    /// Parses a directory for specific file extensions and performs an actin on each file found
    /// </summary>
    /// <param name="directory">Directory to search</param>
    /// <param name="fileExtension">File extension to filter for</param>
    /// <param name="action">Action (function) to perform on each file found</param>
    /// <returns>List of Audiobooks found and parsed</returns>
    /// <inheritdoc cref="System.IO.Directory.GetFiles" path="/exception"/>/>
    /// <inheritdoc cref="System.Linq.Enumerable.Where" path="/exception"/>/>
    /// <inheritdoc cref="System.Linq.Enumerable.Select" path="/exception"/>/>
    /// <inheritdoc cref="MetadataHelper.ExtensionMethods.DiscardNullValues" path="/exception"/>/>
    /// <inheritdoc cref="System.Linq.Enumerable.ToList" path="/exception"/>/>
    private static List<Audiobook> GenericParseDirectory(string directory, string fileExtension, Func<string, Audiobook?> action)
    {
        return Directory.GetFiles(directory, fileExtension)
                        .Select(f => action(f))
                        .DiscardNullValues()
                        .ToList();
    }

    /// <summary>
    /// Renames files with missing series numbers in the ID3 tag to make
    /// them easier to find.
    /// </summary>
    /// <param name="audiobooks">List of Audiobooks to rename.</param>
    /// <inheritdoc cref="System.IO.File.Move" path="/exception"/>
    /// <inheritdoc cref="System.Linq.Enumerable.Where" path="/exception"/>
    /// <inheritdoc cref="System.Linq.Enumerable.AsEnumerable" path="/exception"/>
    public static void RenameMissingSeriesNumber(ICollection<Audiobook> audiobooks)
    {
        var appendString = $"{DateTime.Now:yyyy-MM-dd_HHmmss}";
        foreach (Audiobook audiobook in audiobooks.Where(ab => !string.IsNullOrEmpty(ab.Series)
                                                              && string.IsNullOrEmpty(ab.BookNumber)).AsEnumerable())
        {
            File.Move(audiobook.Filename, $"{audiobook.Filename}{appendString}");
        }
    }

}