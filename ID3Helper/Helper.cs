using IdSharp.Tagging.ID3v2;
using System.Text;

namespace ID3Helper
{
    public static class Helper
    {
        /// <summary>
        /// Parses the ID3 tags of an audiobook and returns an <see cref="Audiobook"/> object if parsing
        /// was successful
        /// </summary>
        /// <param name="filename">Full file path</param>
        /// <returns><see cref="Audiobook"/> object if successful, <see langword="null"/> otherwise.</returns>
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

        /// <summary>
        /// Get all mp3 files from a directory and parse the ID3 tags of
        /// each one then return the resulting list.
        /// </summary>
        /// <param name="directory">Directory to parse</param>
        /// <returns>List of <see cref="Audiobook"/> objects parsed from the directory.</returns>
        /// <inheritdoc cref="System.IO.Directory.GetFiles" path="/exception"/>
        /// <inheritdoc cref="System.Linq.Enumerable.Select" path="/exception"/>
        /// <inheritdoc cref="System.Linq.Enumerable.ToList" path="/exception"/>
        public static List<Audiobook> ParseDirectory(string directory)
        {
            return Directory.GetFiles(directory, "*.mp3")
                            .Select(f => ParseID3Tags(f))
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
            foreach(Audiobook audiobook in audiobooks.Where(ab => !string.IsNullOrEmpty(ab.Series)
                                                                  && string.IsNullOrEmpty(ab.BookNumber)).AsEnumerable())
            {
                File.Move(audiobook.Filename, $"{audiobook.Filename}{appendString}");
            }
        }
    }
}