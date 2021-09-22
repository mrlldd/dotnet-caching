using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class CacheStoreProvider<TStoreFlag> : ICacheStoreProvider<TStoreFlag>
        where TStoreFlag : CachingFlag
    {
        public ICacheStore<TStoreFlag> CacheStore { get; }

        public CacheStoreProvider(ICacheStore<TStoreFlag> cacheStore) 
            => CacheStore = cacheStore;
    }
}