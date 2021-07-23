using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IServiceProvider serviceProvider)
            : base(memoryCache,
                distributedCache,
                serviceProvider)
        {
        }

        public ICache<T> Get<T>() 
            => InternalGet<ICache<T>, T>();
    }
}