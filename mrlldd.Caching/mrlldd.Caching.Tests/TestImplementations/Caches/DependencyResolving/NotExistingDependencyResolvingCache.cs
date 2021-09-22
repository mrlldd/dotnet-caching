using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.TestImplementations.Flags;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving
{
    public class NotExistingDependencyResolvingCache : Cache<DependencyResolvingUnit, InNotExistingStore>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "not-existing";
    }
}