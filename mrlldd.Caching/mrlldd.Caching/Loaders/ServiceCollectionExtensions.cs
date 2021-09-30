using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.Internal;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Loaders
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoaders(this IServiceCollection services, ICollection<Type> types)
        {
            services.AddScoped<ILoaderProvider, LoaderProvider>();
            var loaderTypes = types
                .CollectServices(typeof(ICachingLoader<,>), typeof(CachingLoader<,,>), typeof(IInternalLoaderService<,>));
            return loaderTypes
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