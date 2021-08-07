using System;
using System.Collections.Generic;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        public CacheProvider(IServiceProvider serviceProvider,
            IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IBubbleCacheStore bubbleCacheStore,
            IStoreOperationProvider storeOperationProvider,
            IEnumerable<ICacheStoreDecorator> decorators)
            : base(serviceProvider,
                memoryCacheStore,
                distributedCacheStore,
                bubbleCacheStore,
                storeOperationProvider,
                decorators)
        {
        }

        public ICache<T> Get<T>()
            => InternalGet<ICache<T>>();
    }
}