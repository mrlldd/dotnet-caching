using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.TestImplementations.Caches
{
    public class MoqCache : Cache<VoidUnit, InMoq>
    {
        public MoqCache(CachingOptions injected)
        {
            Options = injected;
        }

        protected override CachingOptions Options { get; }
        protected override string CacheKey => "moq";

        protected override string CacheKeySuffix => "singleton";
    }
}