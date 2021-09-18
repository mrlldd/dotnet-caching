using System;
using System.Linq;
using System.Reflection;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Caches
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaches(this IServiceCollection services, params Assembly[] assemblies)
            => services
                .AddMemoryCache()
                .Effect(x => x.TryAddSingleton<IDistributedCache, NoOpDistributedCache>())
                .AddScoped<ICacheProvider, CacheProvider>()
                .AddScoped<ICache, Cache>()
                .Map(sc =>
                {
                    var cacheTypes = assemblies
                        .SelectMany(x => x.CollectServices(typeof(ICache<,>), typeof(Cache<,>),
                            typeof(IInternalCacheService<,>), t => t)
                        )
                        .ToArray();
                    foreach (var r in cacheTypes
                        .Select(x => x.Service.GetGenericArguments().First())
                        .Distinct()
                        .Select(x => new
                        {
                            Service = typeof(ICaches<>).MakeGenericType(x),
                            Implementation = typeof(Caches<>).MakeGenericType(x)
                        }))
                    {
                        sc.AddScoped(r.Service, r.Implementation);
                    }
                    
                    // todo improve caches collection service registration and use
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