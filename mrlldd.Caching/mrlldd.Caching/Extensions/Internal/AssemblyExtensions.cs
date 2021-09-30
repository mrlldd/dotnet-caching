using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace mrlldd.Caching.Extensions.Internal
{
    internal static class AssemblyExtensions
    {
        public static IEnumerable<(Type Implementation, Type Service, Type MarkerInterface)> CollectServices(
            this IEnumerable<Type> types,
            Type baseServiceType,
            Type baseImplementationType,
            Type markerInterfaceType)
        {
            var enumerable = types
                .Where(x => x.IsClass && !x.IsAbstract && !x.IsGenericType &&
                            x.GetInterfaces().Any(i =>
                                i.IsConstructedGenericType && i.GetGenericTypeDefinition() == baseServiceType));
            return enumerable
                .Select(x => (
                    x,
                    baseServiceType.MakeGenericType(GetRequiredBaseGenericImplementationType(x, baseImplementationType).GetGenericArguments()),
                    x.GetInterfaces()
                        .First(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == markerInterfaceType)
                ));
        }

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