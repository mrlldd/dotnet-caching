using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches.Internal
{
    internal interface IInternalCache<T> : ICaching
    {
    }

    internal interface IInternalCache<T, TFlag> : IInternalCache<T> where TFlag : CachingFlag
    {
    }
}