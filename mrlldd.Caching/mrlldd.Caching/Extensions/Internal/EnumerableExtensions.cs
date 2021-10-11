using System;
using System.Collections.Generic;
using System.Linq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caches.Internal;

namespace mrlldd.Caching.Extensions.Internal
{
    internal static class EnumerableExtensions
    {
        public static IReadOnlyCachesCollection<T> ToCachesCollection<T>(this IEnumerable<IUnknownStoreCache<T>> enumerable)
            => new ReadOnlyCachesCollection<T>(enumerable.ToArray());

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}