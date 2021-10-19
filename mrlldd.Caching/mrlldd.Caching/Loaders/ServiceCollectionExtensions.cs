using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Loaders.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, ICollection<Type> types)
        {
            var cachingLoaderTypes = types
                .CollectServices(typeof(ICachingLoader<,,>), typeof(CachingLoader<,,>), typeof(IInternalLoader<,,>));
            var loaderTypes = types
                .CollectServices(typeof(ILoader<,>));
            foreach (var (implementation, service) in loaderTypes) services.TryAddScoped(service, implementation);

            // todo validate fact that all caching loaders will have their loader services in service registration time

            return cachingLoaderTypes
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