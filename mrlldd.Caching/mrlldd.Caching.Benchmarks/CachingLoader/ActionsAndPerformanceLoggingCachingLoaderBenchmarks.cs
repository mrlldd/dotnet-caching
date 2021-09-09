using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class ActionsAndPerformanceLoggingCachingLoaderBenchmarks : Benchmark
    {
        private readonly ICachingLoader<short, string> actionsAndPerfLoggingMemoryAndDistributedCachingLoader;
        private readonly ICachingLoader<byte, string> actionsAndPerfLoggingDistributedCachingLoader;
        private readonly ICachingLoader<int, string> actionsAndPerfLoggingMemoryCachingLoader;

        public ActionsAndPerformanceLoggingCachingLoaderBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCachingLoaderBenchmarks).Assembly)
                .WithActionsLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            actionsAndPerfLoggingMemoryCachingLoader = cleanSp.GetRequiredService<ICachingLoader<int, string>>();
            actionsAndPerfLoggingDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<byte, string>>();
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<short, string>>();
        }

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_GetOrLoad_Sync() => actionsAndPerfLoggingMemoryCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_GetOrLoad_OmitCache_Sync() =>
            actionsAndPerfLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_GetOrLoad_Async() => actionsAndPerfLoggingMemoryCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_GetOrLoad_OmitCache_Async() =>
            actionsAndPerfLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_Get_Sync() => actionsAndPerfLoggingMemoryCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_Get_Async() => actionsAndPerfLoggingMemoryCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_Set_Sync() => actionsAndPerfLoggingMemoryCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_Set_Async() => actionsAndPerfLoggingMemoryCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_Refresh_Sync() => actionsAndPerfLoggingMemoryCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_Refresh_Async() => actionsAndPerfLoggingMemoryCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Memory_Remove_Sync() => actionsAndPerfLoggingMemoryCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Memory_Remove_Async() => actionsAndPerfLoggingMemoryCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_GetOrLoad_Sync() => actionsAndPerfLoggingDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_GetOrLoad_OmitCache_Sync() =>
            actionsAndPerfLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_GetOrLoad_Async() =>
            actionsAndPerfLoggingDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_GetOrLoad_OmitCache_Async() =>
            actionsAndPerfLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_Get_Sync() => actionsAndPerfLoggingDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_Get_Async() => actionsAndPerfLoggingDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_Set_Sync() => actionsAndPerfLoggingDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_Set_Async() => actionsAndPerfLoggingDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_Refresh_Sync() => actionsAndPerfLoggingDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_Refresh_Async() =>
            actionsAndPerfLoggingDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_Distributed_Remove_Sync() => actionsAndPerfLoggingDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_Distributed_Remove_Async() => actionsAndPerfLoggingDistributedCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_GetOrLoad_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_GetOrLoad_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_Get_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_Get_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_Set_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_Set_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_Refresh_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_Refresh_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsAndPerfLogging_MemoryAndDistributed_Remove_Sync() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsAndPerfLogging_MemoryAndDistributed_Remove_Async() =>
            actionsAndPerfLoggingMemoryAndDistributedCachingLoader.RemoveAsync(3);
    }
}