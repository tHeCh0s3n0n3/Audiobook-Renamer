using IdSharp.Tagging.ID3v2;
using System.Text;

namespace ID3Helper
{
    public static class Helper
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

        public static List<Audiobook> ParseDirectory(string directory)
        {
            return Directory.GetFiles(directory, "*.mp3")
                            .Select(f => ParseID3Tags(f))
                            .DiscardNullValues()
                            .ToList();
        }

        public static void RenameMissingSeriesNumber(List<Audiobook> audiobooks)
        {
            var appendString = $"{DateTime.Now:yyyy-MM-dd_HHmmss}";
            foreach(Audiobook audiobook in audiobooks.Where(ab => !string.IsNullOrEmpty(ab.Series)
                                                                  && string.IsNullOrEmpty(ab.BookNumber)).AsEnumerable())
            {
                File.Move(audiobook.Filename, $"{audiobook.Filename}{appendString}");
            }
        }
    }
}