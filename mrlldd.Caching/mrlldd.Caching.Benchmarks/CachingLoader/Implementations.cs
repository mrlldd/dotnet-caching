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

        protected override Task<string?> LoadAsync(int args, CancellationToken token = default)
            => Task.FromResult(args.ToString())!;

        protected override string CacheKeySuffixFactory(int args)
            => args.ToString();
    }
    
    public class ImplementedDistributedCachingLoader : CachingLoader<byte, string, InDistributed>
    {
        protected override CachingOptions Options => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => "byte-string";

        protected override Task<string?> LoadAsync(byte args, CancellationToken token = default)
            => Task.FromResult(args.ToString())!;

        protected override string CacheKeySuffixFactory(byte args)
            => args.ToString();
    }
}