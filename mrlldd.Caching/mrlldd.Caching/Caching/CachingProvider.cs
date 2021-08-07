using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ICachingStoreDecorator[] decorators;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();
        private readonly IBubbleCachingStore bubbleCachingStore;
        private readonly IMemoryCachingStore memoryCachingStore;
        private readonly IDistributedCachingStore distributedCachingStore;

        protected CachingProvider(IServiceProvider serviceProvider,
            IMemoryCachingStore memoryCachingStore,
            IDistributedCachingStore distributedCachingStore,
            IBubbleCachingStore bubbleCachingStore,
            IEnumerable<ICachingStoreDecorator> decorators)
        {
            this.serviceProvider = serviceProvider;
            this.memoryCachingStore = memoryCachingStore;
            this.distributedCachingStore = distributedCachingStore;
            this.bubbleCachingStore = bubbleCachingStore;
            this.decorators = decorators
                .OrderBy(x => x.Order)
                .ToArray();
        }

        private void Populate<T>(T target) where T : ICaching 
            => target.Populate(
                target.IsUsingMemory
                    ? decorators.Aggregate(memoryCachingStore, (store, decorator) => decorator.Decorate(store))
                    : bubbleCachingStore,
                target.IsUsingDistributed
                    ? decorators.Aggregate(distributedCachingStore, (store, decorator) => decorator.Decorate(store))
                    : bubbleCachingStore
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