using System;
using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Tests.TestUtilities;

namespace mrlldd.Caching.Tests.Loaders.TestUtilities
{
    public class TestLoader : CachingLoader<TestArgument, TestUnit>
    {
        private readonly ITestClient client;
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.MaxValue);
        protected override string CacheKey => "argumentToUnit";
        
        public TestLoader(ITestClient client) 
            => this.client = client;

        protected override Task<TestUnit> LoadAsync(TestArgument args, CancellationToken token = default)
            => client.LoadAsync(args);
        protected override string CacheKeySuffixFactory(TestArgument args) 
            => args.Id.ToString();
    }
}