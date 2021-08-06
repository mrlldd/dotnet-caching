using System;
using System.Collections.Generic;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();

        protected CachingProvider(IServiceProvider serviceProvider) 
            => this.serviceProvider = serviceProvider;

        private void Populate<T, TCached>(T target) where T : ICaching<TCached>
        {
            IBubbleCachingStore bubbleCachingStore = null;
            target.Populate(
                target.IsUsingMemory
                    ? serviceProvider.GetRequiredService<IMemoryCachingStore>()
                    : bubbleCachingStore = serviceProvider.GetRequiredService<IBubbleCachingStore>(),
                target.IsUsingDistributed
                    ? serviceProvider.GetRequiredService<IDistributedCachingStore>()
                    : bubbleCachingStore ?? serviceProvider.GetRequiredService<IBubbleCachingStore>(),
                serviceProvider.GetRequiredService<ILogger<ICaching<TCached>>>()
            );
        }

        protected T InternalGet<T, TCached>() where T : ICaching<TCached>
            => scopedServicesCache.TryGetValue(typeof(T), out var raw)
               && raw is T service
                ? service
                : serviceProvider
                    .GetRequiredService<T>()
                    .Effect(Populate<T, TCached>)
                    .Effect(x => scopedServicesCache[typeof(T)] = x);
    }
}