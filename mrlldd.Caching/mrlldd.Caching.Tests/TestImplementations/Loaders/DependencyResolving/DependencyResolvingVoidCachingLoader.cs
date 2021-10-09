using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Loaders.DependencyResolving
{
    public class DependencyResolvingVoidCachingLoader : CachingLoader<DependencyResolvingUnit, string, InVoid>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "void";

        protected override string CacheKeySuffixFactory(DependencyResolvingUnit args)
            => args.ToString()!;
    }
}