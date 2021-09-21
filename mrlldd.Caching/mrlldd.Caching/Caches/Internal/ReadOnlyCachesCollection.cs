using System.Collections;
using System.Collections.Generic;
using System.Linq;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches.Internal
{
    internal class ReadOnlyCachesCollection<T> : IReadOnlyCachesCollection<T>
    {
        private readonly IReadOnlyCollection<IUnknownStoreCache<T>> collection;

        public ReadOnlyCachesCollection(IReadOnlyCollection<IUnknownStoreCache<T>> collection)
            => this.collection = collection;

        public IEnumerable<ICache<T, TFlag>> WithFlag<TFlag>()
            where TFlag : CachingFlag
            => collection.OfType<ICache<T, TFlag>>();

        public IEnumerator<IUnknownStoreCache<T>> GetEnumerator()
            => collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => collection.GetEnumerator();

        public int Count => collection.Count;
    }
}