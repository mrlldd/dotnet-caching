using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal interface ICacheStoreProvider<TStoreFlag> where TStoreFlag : CachingFlag
    {
        ICacheStore<TStoreFlag> CacheStore { get; }
    }
}