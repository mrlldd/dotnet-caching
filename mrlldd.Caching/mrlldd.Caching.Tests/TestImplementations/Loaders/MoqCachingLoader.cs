using Bogus.DataSets;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Loaders
{
    public class MoqCachingLoader : CachingLoader<VoidUnit, VoidUnit, InMoq>
    {
        protected override CachingOptions Options { get; }
        protected override string CacheKey => "moq";

        public MoqCachingLoader(CachingOptions injected)
            => Options = injected;

        protected override string CacheKeySuffixFactory(VoidUnit args)
            => nameof(args);
    }
}