using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class CleanCacheBenchmarks : Benchmark
    {
        private readonly ICache<byte, InDistributed> cleanDistributedCacheImplementation;
        private readonly ICache<int, InMemory> cleanMemoryCacheImplementation;

        public CleanCacheBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCacheBenchmarks).Assembly)
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            cleanMemoryCacheImplementation = cleanSp.GetRequiredService<ICache<int, InMemory>>();
            cleanDistributedCacheImplementation = cleanSp.GetRequiredService<ICache<byte, InDistributed>>();
        }

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Set_Sync()
        {
            cleanMemoryCacheImplementation.Set(3);
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanMemoryCacheImplementation_Set_Async()
        {
            return cleanMemoryCacheImplementation.SetAsync(3);
        }

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Get_Sync()
        {
            cleanMemoryCacheImplementation.Get();
        }

        [Benchmark]
        public ValueTask<Result<int>> Cache_Caching_CleanMemoryCacheImplementation_Get_Async()
        {
            return cleanMemoryCacheImplementation.GetAsync();
        }

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Refresh_Sync()
        {
            cleanMemoryCacheImplementation.Refresh();
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanMemoryCacheImplementation_Refresh_Async()
        {
            return cleanMemoryCacheImplementation.RefreshAsync();
        }

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Remove_Sync()
        {
            cleanMemoryCacheImplementation.Remove();
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanMemoryCacheImplementation_Remove_Async()
        {
            return cleanMemoryCacheImplementation.RemoveAsync();
        }

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Set_Sync()
        {
            cleanDistributedCacheImplementation.Set(3);
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanDistributedCacheImplementation_Set_Async()
        {
            return cleanDistributedCacheImplementation.SetAsync(3);
        }

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Get_Sync()
        {
            cleanDistributedCacheImplementation.Get();
        }

        [Benchmark]
        public ValueTask<Result<byte>> Cache_Caching_CleanDistributedCacheImplementation_Get_Async()
        {
            return cleanDistributedCacheImplementation.GetAsync();
        }

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Refresh_Sync()
        {
            cleanDistributedCacheImplementation.Refresh();
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanDistributedCacheImplementation_Refresh_Async()
        {
            return cleanDistributedCacheImplementation.RefreshAsync();
        }

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Remove_Sync()
        {
            cleanDistributedCacheImplementation.Remove();
        }

        [Benchmark]
        public ValueTask<Result> Cache_Caching_CleanDistributedCacheImplementation_Remove_Async()
        {
            return cleanDistributedCacheImplementation.RemoveAsync();
        }
    }
}