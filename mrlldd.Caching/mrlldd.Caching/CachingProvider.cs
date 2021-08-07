using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICacheStoreDecorator[] decorators;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();
        private readonly IBubbleCacheStore bubbleCacheStore;
        private readonly IStoreOperationProvider storeOperationProvider;
        private readonly IMemoryCacheStore memoryCacheStore;
        private readonly IDistributedCacheStore distributedCacheStore;

        protected CachingProvider(IServiceProvider serviceProvider,
            IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IBubbleCacheStore bubbleCacheStore,
            IStoreOperationProvider storeOperationProvider,
            IEnumerable<ICacheStoreDecorator> decorators)
        {
            this.serviceProvider = serviceProvider;
            this.memoryCacheStore = memoryCacheStore;
            this.distributedCacheStore = distributedCacheStore;
            this.bubbleCacheStore = bubbleCacheStore;
            this.storeOperationProvider = storeOperationProvider;
            this.decorators = decorators
                .OrderBy(x => x.Order)
                .ToArray();
        }

        private void Populate<T>(T target) where T : ICaching 
            => target.Populate(
                target.IsUsingMemory
                    ? decorators.Aggregate(memoryCacheStore, (store, decorator) => decorator.Decorate(store))
                    : bubbleCacheStore,
                target.IsUsingDistributed
                    ? decorators.Aggregate(distributedCacheStore, (store, decorator) => decorator.Decorate(store))
                    : bubbleCacheStore,
                storeOperationProvider
            );

        protected T InternalGet<T>() where T : ICaching
            => scopedServicesCache.TryGetValue(typeof(T), out var raw)
               && raw is T service
                ? service
                : serviceProvider
                    .GetRequiredService<T>()
                    .Effect(Populate)
                    .Effect(x => scopedServicesCache[typeof(T)] = x);
    }
}