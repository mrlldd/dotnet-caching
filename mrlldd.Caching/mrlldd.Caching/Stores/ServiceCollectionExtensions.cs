using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The class that contains extensions methods for dependency injection of caching stores.
    /// </summary>
    internal static class ServiceCollectionExtensions
    {
        /// <summary>
        /// The method for registering caching stores used by caches and loaders.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddCachingStores(this IServiceCollection services)
            => services
                .UseCachingStore<InMemory, MemoryCacheStore>(ServiceLifetime.Singleton)
                .UseCachingStore<InDistributed, DistributedCacheStore>()
                .UseCachingStore<InVoid, VoidCacheStore>(ServiceLifetime.Singleton);

        public static IServiceCollection UseCachingStore<TFlag, TStore>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStore : ICacheStore<TFlag>
            where TFlag : CachingFlag
        {
            var storeDescriptor = ServiceDescriptor.Describe(typeof(ICacheStore<TFlag>), typeof(TStore), serviceLifetime);
            services.Add(storeDescriptor);
            services.AddScoped<ICacheStoreProvider<TFlag>>(sp =>
            {
                var decorators = sp.GetRequiredService<IEnumerable<ICacheStoreDecorator<TFlag>>>()
                    .ToArray();
                Array.Sort(decorators, DecoratorComparer<TFlag>.Instance);
                var s = sp.GetRequiredService<ICacheStore<TFlag>>();
                for (var i = 0; i < decorators.Length; i++)
                {
                    s = decorators[i].Decorate(s);
                }

                return new CacheStoreProvider<TFlag>(s);
            });
            return services;
        }
    }
}