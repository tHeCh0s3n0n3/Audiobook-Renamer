using MetadataHelper;
using System.Text;

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
                        var tif = id3Tag as TagLib.Id3v2.UserTextInformationFrame;
                        if (tif is not null)
                        {
                            string? desc = tif.Description;
                            string[] tift = tif.Text;
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
                                    int encoding = tif.TextEncoding switch
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
                    }
                }
            }

            if (string.IsNullOrEmpty(author))
            {
                return null;
            }

            return new Audiobook(filename, utfJson, title, author, seriesName, bookNumber);
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
