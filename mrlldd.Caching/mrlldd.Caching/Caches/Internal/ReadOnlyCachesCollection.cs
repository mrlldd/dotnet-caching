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
        {
            this.collection = collection;
        }

        public IEnumerable<ICache<T, TFlag>> WithFlag<TFlag>()
            where TFlag : CachingFlag
        {
            return collection.OfType<ICache<T, TFlag>>();
        }

        public IEnumerator<IUnknownStoreCache<T>> GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return collection.GetEnumerator();
        }

        public int Count => collection.Count;
    }
}