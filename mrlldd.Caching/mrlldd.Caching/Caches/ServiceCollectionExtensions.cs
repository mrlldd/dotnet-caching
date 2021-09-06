using System.Linq;
using System.Reflection;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Caches
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, Assembly assembly)
            => services
                .AddMemoryCache()
                .Effect(x => x.TryAddSingleton<IDistributedCache, NoOpDistributedCache>())
                .AddScoped<ICacheProvider, CacheProvider>()
                .AddScoped<ICache, Cache>()
                .AddSingleton<ICacheOptions>(new NullDefaultCacheOptions())
                .Map(sc =>
                {
                    var cacheTypes = assembly
                        .CollectServices(typeof(ICache<>), typeof(Cache<>),
                            typeof(IInternalCacheService<>));
                    return cacheTypes
                        .Aggregate(sc, (prev, next) =>
                        {
                            var (implementation, service, markerInterface) = next;
                            return prev
                                .AddScoped(markerInterface, implementation)
                                .AddScoped(service,
                                    sp =>
                                    {
                                        var result = sp.GetRequiredService<ICacheProvider>()
                                            .GetRequired(markerInterface);
                                        return result.Successful
                                            ? result.UnwrapAsSuccess()
                                            : throw result;
                                    });
                        });
                });
    }
}