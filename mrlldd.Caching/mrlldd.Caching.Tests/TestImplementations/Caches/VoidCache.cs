using mrlldd.Caching.Caches;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches
{
    public class VoidCache : Cache<VoidUnit, InVoid>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "void";
    }
}