using MetadataHelper;
using System.Text;
using TagLib.Id3v2;

namespace Benchmarks;

public static class TagLibSharpTest
{
    public static Audiobook? ParseID3Tags(string filename)
    {
        try
        {
            string? title = null;
            string? author = null;
            string? seriesName = null;
            string? bookNumber = null;
            string? utfJson = null;
            List<AudiobookChapter> chapters = new();

            TagLib.File tagFile = TagLib.File.Create(filename);

            title = tagFile.Tag.Title;
            foreach (TagLib.Tag tagContainer in (tagFile.Tag as TagLib.NonContainer.Tag)?.Tags ?? Array.Empty<TagLib.Tag>())
            {
                if (tagContainer.TagTypes == TagLib.TagTypes.Id3v2)
                {
                    if (tagContainer is not TagLib.Id3v2.Tag id3Tags)
                    {
                        continue;
                    }
                    foreach (var id3Tag in id3Tags)
                    {
                        var utif = id3Tag as TagLib.Id3v2.UserTextInformationFrame;
                        if (utif is not null)
                        {
                            string? desc = utif.Description;
                            string[] tift = utif.Text;
                            string? tiftStr = string.Join(", ", tift);
                            if (desc is not null
                                && tiftStr is not null)
                            {
                                if (desc.ToUpper().Equals("AUTHOR"))
                                {
                                    author = tiftStr;
                                }
                                if (desc.ToUpper().Equals("SERIES"))
                                {
                                    seriesName = tiftStr;
                                }
                                if (desc.ToUpper().Equals("SERIES-PART"))
                                {
                                    bookNumber = tiftStr;
                                }
                                if (desc.ToUpper().Equals("JSON64"))
                                {
                                    int encoding = utif.TextEncoding switch
                                    {
                                        TagLib.StringType.Latin1 => 28591,
                                        TagLib.StringType.UTF16 => 1200,
                                        TagLib.StringType.UTF16LE => 1200,
                                        TagLib.StringType.UTF16BE => 1201,
                                        TagLib.StringType.UTF8 => 65001,
                                        _ => 65001
                                    };
                                    utfJson = Encoding.GetEncoding(encoding)
                                                      .GetString(Convert.FromBase64String(tiftStr));
                                }
                            }
                        }

                        if (id3Tag is TagLib.Id3v2.ChapterFrame cif)
                        {
                            chapters.Add(new((cif.SubFrames.FirstOrDefault() as TextInformationFrame)?.Text.FirstOrDefault() ?? string.Empty
                                             , TimeSpan.FromMilliseconds(cif.StartMilliseconds)
                                             , TimeSpan.FromMilliseconds(cif.EndMilliseconds)
                                             , TimeSpan.FromMilliseconds(cif.EndMilliseconds - cif.StartMilliseconds)));
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(author))
            {
                return null;
            }

            return new Audiobook(filename
                                 , utfJson
                                 , title
                                 , author
                                 , seriesName
                                 , bookNumber
                                 , (chapters.Any()
                                    ? chapters.OrderBy(c => c.Start).ToArray()
                                    : null));
        }
        catch
        {
            return null;
        }
    }

    public static List<Audiobook> ParseDirectory(string directory, MetadataHelper.Helper.FileTypes fileTypes)
    {
        if (fileTypes.HasFlag(MetadataHelper.Helper.FileTypes.MP3))
        {
            return GenericParseDirectory(directory, "*.mp3", ParseID3Tags);
        }

        if (fileTypes.HasFlag(MetadataHelper.Helper.FileTypes.M4A))
        {
            return GenericParseDirectory(directory, "*.m4a", ParseID3Tags);
        }

        if (fileTypes.HasFlag(MetadataHelper.Helper.FileTypes.M4B))
        {
            return GenericParseDirectory(directory, "*.m4b", ParseID3Tags);
        }

        return new();
    }

    private static List<Audiobook> GenericParseDirectory(string directory, string fileExtension, Func<string, Audiobook?> action)
    {
        return Directory.GetFiles(directory, fileExtension)
                        .Select(f => action(f))
                        .DiscardNullValues()
                        .ToList();
    }
}
