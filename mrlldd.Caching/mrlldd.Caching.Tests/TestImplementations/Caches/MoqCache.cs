using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches
{
    public class MoqCache : Cache<VoidUnit, InMoq>
    {
        protected override CachingOptions Options { get; }
        protected override string CacheKey => "moq";

        protected override string DefaultKeySuffix => "singleton";

        public MoqCache(CachingOptions injected) 
            => Options = injected;
    }
}