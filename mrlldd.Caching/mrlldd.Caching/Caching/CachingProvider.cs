using System;
using System.Collections.Generic;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Caching
{
    internal abstract class CachingProvider
    {
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly IServiceProvider serviceProvider;
        private readonly IDictionary<Type, object> scopedServicesCache = new Dictionary<Type, object>();

        protected CachingProvider(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            IServiceProvider serviceProvider)
        {
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.serviceProvider = serviceProvider;
        }

        private void Populate<T, TCached>(T target) where T : ICaching<TCached>
            => target
                .Populate(memoryCache,
                    distributedCache,
                    serviceProvider.GetRequiredService<ILogger<ICaching<TCached>>>());

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