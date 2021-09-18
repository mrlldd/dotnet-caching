using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Internal
{
    internal interface IInternalCacheService<T, TFlag> where TFlag : CachingFlag
    {
    }
}