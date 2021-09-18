using System;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class ImplementedMemoryCache : Cache<int, InMemory>
    {
        protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override string CacheKey => nameof(Int32);
    }
        
    public class ImplementedDistributedCache : Cache<byte, InDistributed>
    {
        protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override string CacheKey => nameof(Byte);
    }
}