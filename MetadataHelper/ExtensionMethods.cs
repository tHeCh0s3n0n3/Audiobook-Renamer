namespace MetadataHelper;

public static class ExtensionMethods
{    /// <summary>
     /// Discards all null elements of the IEnumerable
     /// </summary>
     /// <param name="enumerable">The IEnumerable itself</param>
     /// <returns>A new <see cref="System.Collections.Generic.IEnumerable{T}"/> of type <typeparamref name="T"/> with all null elements removed.</returns>
     /// <inheritdoc cref="Enumerable.Where{TSource}(IEnumerable{TSource}, Func{TSource, bool})" path="/exception"/>
    public static IEnumerable<T> DiscardNullValues<T>(this IEnumerable<T?> enumerable)
    {
        foreach (T? item in enumerable.Where(i => i is not null))
        {
            yield return item!;
        }
    }
}
