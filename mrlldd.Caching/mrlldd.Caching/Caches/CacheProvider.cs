using System;
using System.Collections.Generic;
using Functional.Object.Extensions;
using Functional.Result;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Caches
{
    internal sealed class CacheProvider : CachingProvider, ICacheProvider
    {
        private readonly ICacheOptions defaultCacheOptions;

        public CacheProvider(IServiceProvider serviceProvider,
            IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IBubbleCacheStore bubbleCacheStore,
            IStoreOperationProvider storeOperationProvider,
            IEnumerable<ICacheStoreDecorator> decorators,
            ICacheOptions defaultCacheOptions)
            : base(serviceProvider,
                memoryCacheStore,
                distributedCacheStore,
                bubbleCacheStore,
                storeOperationProvider,
                decorators) 
            => this.defaultCacheOptions = defaultCacheOptions;

        public Result<ICache<T>> GetRequired<T>()
            => InternalRequiredGet<ICache<T>>();

        public Result<object> GetRequired(Type type)
            => InternalRequiredGet(type);
        
        public ICache<T> GetOrDefault<T>()
            => InternalGet<ICache<T>>()
                .Map(x => x ?? new DefaultCache<T>(defaultCacheOptions));
    }
}