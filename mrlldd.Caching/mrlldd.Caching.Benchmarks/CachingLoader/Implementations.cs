using System;
using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class ImplementedMemoryCachingLoader : CachingLoader<int, string, InMemory>
    {
        protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => "int-string";

        protected override string CacheKeySuffixFactory(int args)
        {
            return args.ToString();
        }
    }

    public class MemoryLoader : ILoader<int, string>
    {
        public Task<string> LoadAsync(int args, CancellationToken token = default)
        {
            return Task.FromResult(args.ToString());
        }
    }

    public class ImplementedDistributedCachingLoader : CachingLoader<byte, string, InDistributed>
    {
        protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => "byte-string";

        protected override string CacheKeySuffixFactory(byte args)
        {
            return args.ToString();
        }
    }

    public class DistributedLoader : ILoader<byte, string>
    {
        public Task<string> LoadAsync(byte args, CancellationToken token = default)
        {
            return Task.FromResult(args.ToString());
        }
    }
}