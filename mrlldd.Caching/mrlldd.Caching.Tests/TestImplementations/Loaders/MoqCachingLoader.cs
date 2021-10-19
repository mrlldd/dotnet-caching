using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Loaders
{
    public class MoqCachingLoader : CachingLoader<VoidUnit, VoidUnit, InMoq>
    {
        public MoqCachingLoader(CachingOptions injected)
        {
            Options = injected;
        }

        protected override CachingOptions Options { get; }
        protected override string CacheKey => "moq";

        protected override string CacheKeySuffixFactory(VoidUnit args)
        {
            return nameof(args);
        }
    }
}