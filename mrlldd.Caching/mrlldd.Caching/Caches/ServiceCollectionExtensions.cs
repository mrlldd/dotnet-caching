using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Extensions.Internal;

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
                            typeof(IInternalCache<,>), t => t)
                        )
                        .ToArray();
                    var noflagTypes = cacheTypes
                        .Select(x => x.Service.GetGenericArguments()[0].Map(t => new
                        {
                            NoflagService = typeof(IUnknownStoreCache<>).MakeGenericType(t),
                            Marker = x.MarkerInterface,
                            GenericArgument = t
                        }))
                        .ToArray();

                    foreach (var type in noflagTypes.Select(x => x.GenericArgument).Distinct())
                    {
                        sc.AddScoped(typeof(ICache<>).MakeGenericType(type), typeof(Cache<>).MakeGenericType(type));
                    }

                    foreach (var item in noflagTypes)
                    {
                        sc.AddScoped(item.NoflagService, sp => sp.GetRequiredService(item.Marker));
                    }

                    var toArrayMethod = typeof(Enumerable)
                        .GetMethod(nameof(Enumerable.ToArray))!;
                    foreach (var type in noflagTypes.Select(x => x.NoflagService).Distinct())
                    {
                        var method = toArrayMethod.MakeGenericMethod(type);
                        var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(type);
                        sc.AddScoped(typeof(IReadOnlyCollection<>).MakeGenericType(type),
                            sp =>
                            {
                                var enumerable = sp.GetRequiredService(genericEnumerable);
                                return method.Invoke(enumerable, new[] { enumerable });
                            });
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