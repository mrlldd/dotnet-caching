using mrlldd.Caching.Caches;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.TestImplementations.Flags;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving
{
    public class GenericDependencyResolvingCache<T, TFlag> : Cache<T, TFlag>
        where TFlag : CachingFlag
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => $"generic-{typeof(T).FullName}-{typeof(TFlag).Name}";
    }

    public class
        GenericImplDependencyResolvingCache : GenericDependencyResolvingCache<DependencyResolvingUnit, InGenericScope>
    {
    }
}