using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Tests.InvalidAssembly.Loaders
{
    public class NoLoaderCachingLoader : CachingLoader<InvalidUnit, InvalidUnit, InVoid>
    {
        protected override CachingOptions Options => CachingOptions.Disabled;
        protected override string CacheKey => "broken";

        protected override string CacheKeySuffixFactory(InvalidUnit args)
        {
            return args.ToString();
        }
    }
}