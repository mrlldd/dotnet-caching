using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Loaders.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, ICollection<Type> types)
        {
            services.AddScoped<ILoaderProvider, LoaderProvider>();
            var cachingLoaderTypes = types
                .CollectServices(typeof(ICachingLoader<,,>), typeof(CachingLoader<,,>), typeof(IInternalLoaderService<,,>));
            var loaderTypes = types
                .CollectServices(typeof(ILoader<,>));
            foreach (var (implementation, service) in loaderTypes)
            {
                services.AddScoped(service, implementation);
            }
            
            // todo validate fact that all caching loaders will have their loader services in service registration time
            
            return cachingLoaderTypes
                .Aggregate(services, (prev, next) =>
                {
                    var (implementation, service, markerInterface) = next;
                    return prev
                        .AddScoped(markerInterface, implementation)
                        .AddScoped(service,
                            sp =>
                            {
                                var result = sp.GetRequiredService<ILoaderProvider>()
                                    .GetRequired(markerInterface);
                                return result.Successful
                                    ? result.UnwrapAsSuccess()
                                    : throw result;
                            });
                });
        }
    }
}