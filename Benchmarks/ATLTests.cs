﻿using ATL;
using MetadataHelper;
using System.Text;

namespace Benchmarks;

public class ATLTests
{
    public static Audiobook? ParseFile(string filename)
    {
        try
        {
            Track track = new(filename);

            string? title = track.Title;
            string? author = track.AlbumArtist;
            string? seriesName = string.IsNullOrEmpty(track.SeriesTitle)
                                 ? track.AdditionalFields.TryGetValue("SERIES", out string? seriesTitle)
                                   ? seriesTitle
                                   : string.Empty
                                 : track.SeriesTitle;
            string? bookNumber = string.IsNullOrEmpty(track.SeriesPart)
                                 ? track.AdditionalFields.TryGetValue("SERIES-PART", out string? seriesPart)
                                   ? seriesPart
                                   : string.Empty
                                 : track.SeriesPart;
            
            string? utfJson = null;
            if (track.AdditionalFields.TryGetValue("json64", out string? json)
                && json is not null)
            {
                utfJson = Encoding.UTF8.GetString(Convert.FromBase64String(json));
            }

            if (string.IsNullOrEmpty(author))
            {
                return null;
            }

            IList<AudiobookChapter> chapters = new AudiobookChapter[track.Chapters.Count];

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

            return new Audiobook(filename, utfJson, title, author, seriesName, bookNumber, chapters);
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
            return GenericParseDirectory(directory, "*.mp3", ParseFile);
        }

        if (fileTypes.HasFlag(MetadataHelper.Helper.FileTypes.M4A))
        {
            return GenericParseDirectory(directory, "*.m4a", ParseFile);
        }

        if (fileTypes.HasFlag(MetadataHelper.Helper.FileTypes.M4B))
        {
            return GenericParseDirectory(directory, "*.m4b", ParseFile);
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
