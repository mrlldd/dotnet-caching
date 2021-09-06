using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Benchmarks.Cache
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class ActionsAndPerformanceLoggingCacheBenchmarks
    {
        private readonly ICache<int> actionsAndPerfLoggingMemoryCacheImplementation;
        private readonly ICache<byte> actionsAndPerfLoggingDistributedCacheImplementation;
        private readonly ICache<string> actionsAndPerfLoggingMemoryAndDistributedCacheImplementation;

        public ActionsAndPerformanceLoggingCacheBenchmarks()
        {
            var actionsLoggingSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(ActionsLoggingCacheBenchmarks).Assembly)
                .WithActionsLogging()
                .WithPerformanceLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            actionsAndPerfLoggingMemoryCacheImplementation = actionsLoggingSp.GetRequiredService<ICache<int>>();
            actionsAndPerfLoggingDistributedCacheImplementation = actionsLoggingSp.GetRequiredService<ICache<byte>>();
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation =
                actionsLoggingSp.GetRequiredService<ICache<string>>();
        }


        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Set_Sync() =>
            actionsAndPerfLoggingMemoryCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Set_Async() =>
            actionsAndPerfLoggingMemoryCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Get_Sync() =>
            actionsAndPerfLoggingMemoryCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Get_Async() =>
            actionsAndPerfLoggingMemoryCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Refresh_Sync() =>
            actionsAndPerfLoggingMemoryCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Refresh_Async() =>
            actionsAndPerfLoggingMemoryCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Remove_Sync() =>
            actionsAndPerfLoggingMemoryCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryCacheImplementation_Remove_Async() =>
            actionsAndPerfLoggingMemoryCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Set_Sync() =>
            actionsAndPerfLoggingDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Set_Async() =>
            actionsAndPerfLoggingDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Get_Sync() =>
            actionsAndPerfLoggingDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Get_Async() =>
            actionsAndPerfLoggingDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Refresh_Sync() =>
            actionsAndPerfLoggingDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Refresh_Async() =>
            actionsAndPerfLoggingDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Remove_Sync() =>
            actionsAndPerfLoggingDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingDistributedCacheImplementation_Remove_Async() =>
            actionsAndPerfLoggingDistributedCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Set_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.Set("3");

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Set_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.SetAsync("3");

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Get_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Get_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Refresh_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Refresh_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Remove_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsAndPerfLoggingMemoryAndDistributedCacheImplementation_Remove_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCacheImplementation.RemoveAsync();
    }
}