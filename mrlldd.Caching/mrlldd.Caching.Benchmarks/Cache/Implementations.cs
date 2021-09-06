using System;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class ImplementedMemoryCache : Cache<int>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Disabled;
        protected override string CacheKey => nameof(Int32);
    }
        
    public class ImplementedDistributedCache : Cache<byte>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Disabled;
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => nameof(Byte);
    }
        
    public class ImplementedMemoryAndDistributedCache : Cache<string>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));

        protected override CachingOptions DistributedCacheOptions =>
            CachingOptions.Enabled(TimeSpan.FromMinutes(5));

        protected override string CacheKey => nameof(String);
    }
}