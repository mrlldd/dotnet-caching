using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace mrlldd.Caching.Extensions.Internal
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithCollectedServices(this IServiceCollection services,
            Assembly assembly,
            Type baseImplementationType,
            Type baseServiceType) =>
            assembly
                .GetTypes()
                .Where(x => x.BaseType is {IsConstructedGenericType: true} &&
                            x.BaseType.GetGenericTypeDefinition() == baseImplementationType)
                .Select(x => new
                {
                    Implementation = x,
                    Service = baseServiceType.MakeGenericType(x.BaseType.GetGenericArguments())
                })
                .Aggregate(services, (sc, svc) => sc.AddScoped(svc.Service, svc.Implementation));
    }
}