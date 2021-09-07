using System;
using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class ImplementedMemoryCachingLoader : CachingLoader<int, string>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Disabled;
        protected override string CacheKey => "int-string";

        protected override Task<string?> LoadAsync(int args, CancellationToken token = default)
            => Task.FromResult(args.ToString())!;

        protected override string CacheKeySuffixFactory(int args)
            => args.ToString();
    }
    
    public class ImplementedDistributedCachingLoader : CachingLoader<byte, string>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Disabled;
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => "byte-string";

        protected override Task<string?> LoadAsync(byte args, CancellationToken token = default)
            => Task.FromResult(args.ToString())!;

        protected override string CacheKeySuffixFactory(byte args)
            => args.ToString();
    }

    public class ImplementedMemoryAndDistributedCachingLoader : CachingLoader<short, string>
    {
        protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.FromMinutes(5));
        protected override string CacheKey => "short-string";

        protected override Task<string?> LoadAsync(short args, CancellationToken token = default)
            => Task.FromResult(args.ToString())!;

        protected override string CacheKeySuffixFactory(short args)
            => args.ToString();
    }
}