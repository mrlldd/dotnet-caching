using mrlldd.Caching.Caches;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving
{
    public class DependencyResolvingVoidCache : Cache<DependencyResolvingUnit, InVoid>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "void";
    }
    
    
}