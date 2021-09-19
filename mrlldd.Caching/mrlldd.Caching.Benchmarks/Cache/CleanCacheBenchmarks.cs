using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class CleanCacheBenchmarks : Benchmark
    {
        private readonly ICache<int, InMemory> cleanMemoryCacheImplementation;
        private readonly ICache<byte,  InDistributed> cleanDistributedCacheImplementation;

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
        public void Cache_Caching_CleanMemoryCacheImplementation_Set_Sync() => cleanMemoryCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_CleanMemoryCacheImplementation_Set_Async() =>
            cleanMemoryCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Get_Sync() => cleanMemoryCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryCacheImplementation_Get_Async() =>
            cleanMemoryCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Refresh_Sync() =>
            cleanMemoryCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryCacheImplementation_Refresh_Async() =>
            cleanMemoryCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_CleanMemoryCacheImplementation_Remove_Sync() =>
            cleanMemoryCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryCacheImplementation_Remove_Async() =>
            cleanMemoryCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Set_Sync() =>
            cleanDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_CleanDistributedCacheImplementation_Set_Async() =>
            cleanDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Get_Sync() =>
            cleanDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_CleanDistributedCacheImplementation_Get_Async() =>
            cleanDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Refresh_Sync() =>
            cleanDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_CleanDistributedCacheImplementation_Refresh_Async() =>
            cleanDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_CleanDistributedCacheImplementation_Remove_Sync() =>
            cleanDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_CleanDistributedCacheImplementation_Remove_Async() =>
            cleanDistributedCacheImplementation.RemoveAsync();
    }
}