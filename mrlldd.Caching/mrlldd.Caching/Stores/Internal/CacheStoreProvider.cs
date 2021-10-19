using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class CacheStoreProvider<TStoreFlag> : ICacheStoreProvider<TStoreFlag>
        where TStoreFlag : CachingFlag
    {
        public CacheStoreProvider(ICacheStore<TStoreFlag> cacheStore)
        {
            CacheStore = cacheStore;
        }

        public ICacheStore<TStoreFlag> CacheStore { get; }
    }
}