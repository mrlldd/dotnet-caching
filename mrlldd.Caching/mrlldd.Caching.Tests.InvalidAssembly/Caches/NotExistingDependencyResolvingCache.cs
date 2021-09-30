using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.InvalidAssembly.Flags;

namespace mrlldd.Caching.Tests.InvalidAssembly.Caches
{
    public class NotExistingDependencyResolvingCache : Cache<InvalidUnit, InNotExistingStore>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "not-existing";
    }
}