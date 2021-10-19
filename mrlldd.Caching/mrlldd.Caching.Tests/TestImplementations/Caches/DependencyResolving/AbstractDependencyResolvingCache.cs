using mrlldd.Caching.Caches;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.TestImplementations.Flags;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving
{
    public abstract class AbstractDependencyResolvingCache<T, TFlag> : Cache<T, TFlag> where TFlag : CachingFlag
    {
        protected override CachingOptions Options => CachingOptions.Disabled;

        protected override string CacheKey => $"abstract-{typeof(T).FullName}-{typeof(TFlag).Name}";
    }

    public class AbstractImplDependencyResolvingCache
        : AbstractDependencyResolvingCache<DependencyResolvingUnit, InAbstractScope>
    {
    }
}