using System;
using System.Collections.Generic;
using System.Linq;

namespace mrlldd.Caching.Extensions.Internal
{
    internal static class TypeExtensions
    {
        public static IEnumerable<(Type Implementation, Type Service)> CollectServices(this IEnumerable<Type> types,
            Type serviceType)
            => types
                .Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType &&
                            x.GetInterfaces().Any(i =>
                                i.IsConstructedGenericType && i.GetGenericTypeDefinition() == serviceType))
                .Select(x => (x,
                        serviceType.MakeGenericType(
                            x.GetInterfaces()
                                .First(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == serviceType)
                                .GetGenericArguments()
                        )
                    )
                );

        public static IEnumerable<(Type Implementation, Type Service, Type MarkerInterface)> CollectServices(
            this IEnumerable<Type> types,
            Type baseServiceType,
            Type baseImplementationType,
            Type markerInterfaceType)
            => types
                .Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType &&
                            x.GetInterfaces().Any(i =>
                                i.IsConstructedGenericType && i.GetGenericTypeDefinition() == baseServiceType))
                .Select(x => (
                    x,
                    baseServiceType.MakeGenericType(GetRequiredBaseGenericImplementationType(x, baseImplementationType)
                        .GetGenericArguments()),
                    x.GetInterfaces()
                        .First(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == markerInterfaceType)
                ));

        private static Type GetRequiredBaseGenericImplementationType(Type? inherited, Type targetBaseType)
        {
            while (true)
            {
                if (inherited == null)
                {
                    throw new ArgumentNullException(nameof(inherited));
                }

                if (!inherited.IsConstructedGenericType)
                {
                    inherited = inherited.BaseType;
                    continue;
                }

                if (inherited.GetGenericTypeDefinition() == targetBaseType)
                {
                    return inherited;
                }

                inherited = inherited.BaseType;
            }
        }
    }
}