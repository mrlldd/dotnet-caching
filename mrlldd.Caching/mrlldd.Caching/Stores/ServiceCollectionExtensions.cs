using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    ///     The class that contains extensions methods for dependency injection of caching stores..
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds a caching store with specified flag to service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="serviceLifetime">The service lifetime scope of store.</param>
        /// <typeparam name="TFlag">The caching flag of store.</typeparam>
        /// <typeparam name="TStore">The implementation type of store with specified flag of type <typeparamref name="TFlag" />.</typeparam>
        /// <returns>The service collection.</returns>
        public static IServiceCollection UseCachingStore<TFlag, TStore>(this IServiceCollection services,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
            where TStore : ICacheStore<TFlag>
            where TFlag : CachingFlag
        {
            var storeDescriptor =
                ServiceDescriptor.Describe(typeof(ICacheStore<TFlag>), typeof(TStore), serviceLifetime);

            services.TryAdd(storeDescriptor);
            services.TryAddScoped<ICacheStoreProvider<TFlag>>(sp =>
            {
                var decorators = sp
                    .GetRequiredService<IEnumerable<ICacheStoreDecorator<TFlag>>>()
                    .ToArray();
                Array.Sort(decorators, CacheStoreDecoratorComparer<TFlag>.Instance);
                var s = sp.GetRequiredService<ICacheStore<TFlag>>();
                for (var i = 0; i < decorators.Length; i++) s = decorators[i].Decorate(s);

                return new CacheStoreProvider<TFlag>(s);
            });
            return services;
        }
    }
}