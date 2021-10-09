using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.InvalidAssembly.Flags;

namespace mrlldd.Caching.Tests.InvalidAssembly.Loaders
{
    public class NotExistingDependencyResolvingCachingLoader : CachingLoader<InvalidUnit, InvalidUnit, InNotExistingStore>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "not-existing";

        protected override string CacheKeySuffixFactory(InvalidUnit args)
            => args.ToString();
    }
}