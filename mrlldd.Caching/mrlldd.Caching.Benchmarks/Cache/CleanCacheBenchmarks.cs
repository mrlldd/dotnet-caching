using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Benchmarks.Cache
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class CleanCacheBenchmarks
    {
        private readonly ICache<int> cleanMemoryCacheImplementation;
        private readonly ICache<byte> cleanDistributedCacheImplementation;
        private readonly ICache<string> cleanMemoryAndDistributedCacheImplementation;


        public CleanCacheBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCacheBenchmarks).Assembly)
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            cleanMemoryCacheImplementation = cleanSp.GetRequiredService<ICache<int>>();
            cleanDistributedCacheImplementation = cleanSp.GetRequiredService<ICache<byte>>();
            cleanMemoryAndDistributedCacheImplementation = cleanSp.GetRequiredService<ICache<string>>();
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

        [Benchmark]
        public void Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Set_Sync() =>
            cleanMemoryAndDistributedCacheImplementation.Set("3");

        [Benchmark]
        public Task Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Set_Async() =>
            cleanMemoryAndDistributedCacheImplementation.SetAsync("3");

        [Benchmark]
        public void Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Get_Sync() =>
            cleanMemoryAndDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Get_Async() =>
            cleanMemoryAndDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Refresh_Sync() =>
            cleanMemoryAndDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Refresh_Async() =>
            cleanMemoryAndDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Remove_Sync() =>
            cleanMemoryAndDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_CleanMemoryAndDistributedCacheImplementation_Remove_Async() =>
            cleanMemoryAndDistributedCacheImplementation.RemoveAsync();
    }
}