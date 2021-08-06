using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class OnlyDistributedCache<T> : TestCache<T>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Disabled;
    }
}