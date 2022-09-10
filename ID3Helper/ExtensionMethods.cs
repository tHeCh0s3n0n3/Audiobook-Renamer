namespace ID3Helper;

public static class ExtensionMethods
{
    public static IEnumerable<T> DiscardNullValues<T>(this IEnumerable<T?> list)
    {
        foreach (T? item in list)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}
