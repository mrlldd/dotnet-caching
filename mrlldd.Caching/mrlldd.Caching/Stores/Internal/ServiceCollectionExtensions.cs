using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
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

        /*
        private static IServiceCollection UseCachingStore(this IServiceCollection services,
            Type serviceType,
            Type implementationType,
            Type flagType,
            ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            
            var storeDescriptor = ServiceDescriptor.Describe(serviceType, implementationType, serviceLifetime);
            services.Add(storeDescriptor);
            var providerType = typeof(ICacheStoreProvider<>).MakeGenericType(flagType);
            var decoratorType = typeof(ICacheStoreDecorator<>).MakeGenericType(flagType);
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(decoratorType);

            var decorate = decoratorType.GetMethod(nameof(ICacheStoreDecorator<CachingFlag>.Decorate))!;
            
            
            services.AddScoped(providerType, sp =>
            {
                var decorators = ((IEnumerable)sp.GetRequiredService(enumerableType))
                    .Cast<object>()
                    .ToArray();
                Array.Sort(decorators, OrderComparer.Instance);
                var s = sp.GetRequiredService(serviceType);
                for (var i = 0; i < decorators.Length; i++)
                {
                    s = decorate.Invoke(decorators[i], new[] {s});
                }

                return new CacheStoreProvider<TFlag>(s);
            });
            return services;
        }*/
    }
}