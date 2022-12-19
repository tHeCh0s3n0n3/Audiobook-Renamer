namespace MetadataHelper;

public static class ExtensionMethods
{    /// <summary>
     /// Discards all null elements of the IEnumerable
     /// </summary>
     /// <param name="enumerable">The IEnumerable itself</param>
     /// <returns>A new <see cref="System.Collections.Generic.IEnumerable{T}"/> of type <typeparamref name="T"/> with all null elements removed.</returns>
    public static IEnumerable<T> DiscardNullValues<T>(this IEnumerable<T?> enumerable)
    {
        foreach (T? item in enumerable)
        {
            if (item is not null)
            {
                yield return item;
            }
        }
    }
}
