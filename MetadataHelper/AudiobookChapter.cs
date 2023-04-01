namespace MetadataHelper;

public record class AudiobookChapter(string Title, TimeSpan? Start = null, TimeSpan? End = null, TimeSpan? Duration = null)
{ }
