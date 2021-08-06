using System;
using System.Collections.Generic;
using mrlldd.Caching.Caching;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IServiceProvider serviceProvider,
            IMemoryCachingStore memoryCachingStore,
            IDistributedCachingStore distributedCachingStore,
            IBubbleCachingStore bubbleCachingStore,
            IEnumerable<ICachingStoreDecorator> decorators)
            : base(serviceProvider,
                memoryCachingStore,
                distributedCachingStore,
                bubbleCachingStore,
                decorators)
        {
        }

        public ICache<T> Get<T>()
            => InternalGet<ICache<T>, T>();
    }
}