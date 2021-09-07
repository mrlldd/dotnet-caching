using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Benchmarks.Cache
{
    public class ActionsLoggingCacheBenchmarks : Benchmark
    {
        private readonly ICache<int> actionsLoggingMemoryCacheImplementation;
        private readonly ICache<byte> actionsLoggingDistributedCacheImplementation;
        private readonly ICache<short> actionsLoggingMemoryAndDistributedCacheImplementation;

        public ActionsLoggingCacheBenchmarks()
        {
            var actionsLoggingSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(ActionsLoggingCacheBenchmarks).Assembly)
                .WithActionsLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;

            actionsLoggingMemoryCacheImplementation = actionsLoggingSp.GetRequiredService<ICache<int>>();
            actionsLoggingDistributedCacheImplementation = actionsLoggingSp.GetRequiredService<ICache<byte>>();
            actionsLoggingMemoryAndDistributedCacheImplementation =
                actionsLoggingSp.GetRequiredService<ICache<short>>();
        }


        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryCacheImplementation_Set_Sync() =>
            actionsLoggingMemoryCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryCacheImplementation_Set_Async() =>
            actionsLoggingMemoryCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryCacheImplementation_Get_Sync() =>
            actionsLoggingMemoryCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryCacheImplementation_Get_Async() =>
            actionsLoggingMemoryCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryCacheImplementation_Refresh_Sync() =>
            actionsLoggingMemoryCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryCacheImplementation_Refresh_Async() =>
            actionsLoggingMemoryCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryCacheImplementation_Remove_Sync() =>
            actionsLoggingMemoryCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryCacheImplementation_Remove_Async() =>
            actionsLoggingMemoryCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingDistributedCacheImplementation_Set_Sync() =>
            actionsLoggingDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingDistributedCacheImplementation_Set_Async() =>
            actionsLoggingDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_ActionsLoggingDistributedCacheImplementation_Get_Sync() =>
            actionsLoggingDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingDistributedCacheImplementation_Get_Async() =>
            actionsLoggingDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingDistributedCacheImplementation_Refresh_Sync() =>
            actionsLoggingDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingDistributedCacheImplementation_Refresh_Async() =>
            actionsLoggingDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingDistributedCacheImplementation_Remove_Sync() =>
            actionsLoggingDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingDistributedCacheImplementation_Remove_Async() =>
            actionsLoggingDistributedCacheImplementation.RemoveAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Set_Sync() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.Set(3);

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Set_Async() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.SetAsync(3);

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Get_Sync() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.Get();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Get_Async() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.GetAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Refresh_Sync() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.Refresh();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Refresh_Async() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.RefreshAsync();

        [Benchmark]
        public void Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Remove_Sync() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.Remove();

        [Benchmark]
        public Task Cache_Caching_ActionsLoggingMemoryAndDistributedCacheImplementation_Remove_Async() =>
            actionsLoggingMemoryAndDistributedCacheImplementation.RemoveAsync();
    }
}