using System;
using System.Collections.Generic;
using System.Linq;
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
        public static IServiceCollection AddCaches(this IServiceCollection services, ICollection<Type> types)
        {
            services.AddMemoryCache();
            services.TryAddSingleton<IDistributedCache, NoOpDistributedCache>();
            var cacheTypes = types
                .CollectServices(typeof(ICache<,>), typeof(Cache<,>), typeof(IInternalCache<,>))
                .ToArray();

            var noflagTypes = cacheTypes
                .Select(x =>
                {
                    var ga = x.Service.GetGenericArguments()[0];
                    return new
                    {
                        NoflagService = typeof(IUnknownStoreCache<>).MakeGenericType(ga),
                        Marker = x.MarkerInterface,
                        GenericArgument = ga
                    };
                })
                .ToArray();

            foreach (var type in noflagTypes.Select(x => x.GenericArgument).Distinct())
                services.TryAddScoped(typeof(ICache<>).MakeGenericType(type), typeof(Cache<>).MakeGenericType(type));

            foreach (var item in noflagTypes)
                services.AddScoped(item.NoflagService, sp => sp.GetRequiredService(item.Marker));

            var toReadonlyCachesCollectionMethod = typeof(EnumerableExtensions)
                .GetMethod(nameof(EnumerableExtensions.ToCachesCollection))!;
            foreach (var item in noflagTypes.DistinctBy(x => x.NoflagService))
            {
                var finalToReadonlyCachesCollection =
                    toReadonlyCachesCollectionMethod.MakeGenericMethod(item.GenericArgument);
                var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(item.NoflagService);
                services.TryAddScoped(typeof(IReadOnlyCachesCollection<>).MakeGenericType(item.GenericArgument),
                    sp =>
                    {
                        var enumerable = sp.GetRequiredService(genericEnumerable);
                        return finalToReadonlyCachesCollection.Invoke(enumerable, new[] {enumerable});
                    });
            }

            return cacheTypes
                .Aggregate(services, (prev, next) =>
                {
                    var (implementation, service, markerInterface) = next;
                    prev.TryAddScoped(markerInterface, implementation);
                    prev.TryAddScoped(service,
                        sp =>
                        {
                            var result = sp.GetRequiredService<CachingProvider>()
                                .GetRequired(markerInterface);
                            return result.Successful
                                ? result.UnwrapAsSuccess()
                                : throw result;
                        });
                    return prev;
                });
        }
    }
}