using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class PerformanceLoggingCacheBenchmarks : Benchmark
    {
        private readonly ICache<int> perfLoggingMemoryCacheImplementation;
        private readonly ICache<byte> perfLoggingDistributedCacheImplementation;
        private readonly ICache<short> perfLoggingMemoryAndDistributedCacheImplementation;

        public PerformanceLoggingCacheBenchmarks()
        {
            var perfLoggingSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(PerformanceLoggingCacheBenchmarks).Assembly)
                .WithActionsLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            perfLoggingMemoryCacheImplementation = perfLoggingSp.GetRequiredService<ICache<int>>();
            perfLoggingDistributedCacheImplementation = perfLoggingSp.GetRequiredService<ICache<byte>>();
            perfLoggingMemoryAndDistributedCacheImplementation =
                perfLoggingSp.GetRequiredService<ICache<short>>();
        }


        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryCacheImplementation_Set_Sync() =>
            perfLoggingMemoryCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryCacheImplementation_Set_Async() =>
            perfLoggingMemoryCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryCacheImplementation_Get_Sync() =>
            perfLoggingMemoryCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryCacheImplementation_Get_Async() =>
            perfLoggingMemoryCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryCacheImplementation_Refresh_Sync() =>
            perfLoggingMemoryCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryCacheImplementation_Refresh_Async() =>
            perfLoggingMemoryCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryCacheImplementation_Remove_Sync() =>
            perfLoggingMemoryCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryCacheImplementation_Remove_Async() =>
            perfLoggingMemoryCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingDistributedCacheImplementation_Set_Sync() =>
            perfLoggingDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_PerfLoggingDistributedCacheImplementation_Set_Async() =>
            perfLoggingDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_PerfLoggingDistributedCacheImplementation_Get_Sync() =>
            perfLoggingDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingDistributedCacheImplementation_Get_Async() =>
            perfLoggingDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingDistributedCacheImplementation_Refresh_Sync() =>
            perfLoggingDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingDistributedCacheImplementation_Refresh_Async() =>
            perfLoggingDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingDistributedCacheImplementation_Remove_Sync() =>
            perfLoggingDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingDistributedCacheImplementation_Remove_Async() =>
            perfLoggingDistributedCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Set_Sync() =>
            perfLoggingMemoryAndDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Set_Async() =>
            perfLoggingMemoryAndDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Get_Sync() =>
            perfLoggingMemoryAndDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Get_Async() =>
            perfLoggingMemoryAndDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Refresh_Sync() =>
            perfLoggingMemoryAndDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Refresh_Async() =>
            perfLoggingMemoryAndDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Remove_Sync() =>
            perfLoggingMemoryAndDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_PerfLoggingMemoryAndDistributedCacheImplementation_Remove_Async() =>
            perfLoggingMemoryAndDistributedCacheImplementation.RemoveAsync();
    }
}