using MetadataHelper;
using IdSharp.Tagging.ID3v2;
using System.Text;

namespace Benchmarks;

public static class IdSharpTest
{
    public static Audiobook? ParseID3Tags(string filename)
    {
        try
        {
            IID3v2Tag id3v2 = new ID3v2Tag(filename);
            if (id3v2 is null)
            {
                return null;
            }

            var udfJsonBase64 = id3v2.UserDefinedText?.Where(udt => udt.Description == "json64").FirstOrDefault();
            string? udfJson = null;
            if (udfJsonBase64 is not null)
            {
                int encoding = udfJsonBase64.TextEncoding switch
                {
                    EncodingType.ISO88591 => 28591,
                    EncodingType.Unicode => 1200,
                    EncodingType.UTF16BE => 1201,
                    EncodingType.UTF8 => 65001,
                    _ => 65001
                };
                udfJson = Encoding.GetEncoding(encoding)
                                  .GetString(Convert.FromBase64String(udfJsonBase64.Value));
            }

            string? title = id3v2.Title;
            string? author = id3v2.UserDefinedText?.Where(udt => udt.Description == "author").FirstOrDefault()?.Value;
            string? seriesName = id3v2.UserDefinedText?.Where(udt => udt.Description == "SERIES").FirstOrDefault()?.Value;
            string? bookNumber = id3v2.UserDefinedText?.Where(udt => udt.Description == "SERIES-PART").FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(author))
            {
                return null;
            }

            return new Audiobook(filename, udfJson, title, author, seriesName, bookNumber);
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
