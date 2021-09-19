using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches.Internal
{
    internal interface IInternalCacheService<T, TFlag> : ICache<T> where TFlag : CachingFlag
    {
    }
}