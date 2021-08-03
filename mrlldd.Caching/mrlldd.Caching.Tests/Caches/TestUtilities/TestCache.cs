using System;
using System.Collections.Generic;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    public class TestCache<T> : Cache<T>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override string CacheKey => "testcache";
        protected override string DefaultKeySuffix => typeof(T).Name;

        protected override IEnumerable<string> CacheKeyPrefixesFactory()
        {
            yield return "test";
        }
    }
}