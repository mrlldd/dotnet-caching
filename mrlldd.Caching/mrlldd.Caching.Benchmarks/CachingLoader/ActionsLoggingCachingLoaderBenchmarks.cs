using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class ActionsLoggingCachingLoaderBenchmarks : Benchmark
    {
        private readonly ICachingLoader<short, string> actionsLoggingMemoryAndDistributedCachingLoader;
        private readonly ICachingLoader<byte, string> actionsLoggingDistributedCachingLoader;
        private readonly ICachingLoader<int, string> actionsLoggingMemoryCachingLoader;

        public ActionsLoggingCachingLoaderBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCachingLoaderBenchmarks).Assembly)
                .WithActionsLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            actionsLoggingMemoryCachingLoader = cleanSp.GetRequiredService<ICachingLoader<int, string>>();
            actionsLoggingDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<byte, string>>();
            actionsLoggingMemoryAndDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<short, string>>();
        }

        [Benchmark]
        public void Loader_ActionsLogging_Memory_GetOrLoad_Sync() => actionsLoggingMemoryCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsLogging_Memory_GetOrLoad_OmitCache_Sync() => actionsLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_GetOrLoad_Async() => actionsLoggingMemoryCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_GetOrLoad_OmitCache_Async() => actionsLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsLogging_Memory_Get_Sync() => actionsLoggingMemoryCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_Get_Async() => actionsLoggingMemoryCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_Memory_Set_Sync() => actionsLoggingMemoryCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_Set_Async() => actionsLoggingMemoryCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsLogging_Memory_Refresh_Sync() => actionsLoggingMemoryCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_Refresh_Async() => actionsLoggingMemoryCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_Memory_Remove_Sync() => actionsLoggingMemoryCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Memory_Remove_Async() => actionsLoggingMemoryCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_GetOrLoad_Sync() => actionsLoggingDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_GetOrLoad_OmitCache_Sync() =>
            actionsLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_GetOrLoad_Async() => actionsLoggingDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_GetOrLoad_OmitCache_Async() =>
            actionsLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_Get_Sync() => actionsLoggingDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_Get_Async() => actionsLoggingDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_Set_Sync() => actionsLoggingDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_Set_Async() => actionsLoggingDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_Refresh_Sync() => actionsLoggingDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_Refresh_Async() => actionsLoggingDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_Distributed_Remove_Sync() => actionsLoggingDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsLogging_Distributed_Remove_Async() => actionsLoggingDistributedCachingLoader.RemoveAsync(3);
        
        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_GetOrLoad_Sync() => actionsLoggingMemoryAndDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Sync() =>
            actionsLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_GetOrLoad_Async() => actionsLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Async() =>
            actionsLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_Get_Sync() => actionsLoggingMemoryAndDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_Get_Async() => actionsLoggingMemoryAndDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_Set_Sync() => actionsLoggingMemoryAndDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_Set_Async() => actionsLoggingMemoryAndDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_Refresh_Sync() => actionsLoggingMemoryAndDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_Refresh_Async() => actionsLoggingMemoryAndDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_ActionsLogging_MemoryAndDistributed_Remove_Sync() => actionsLoggingMemoryAndDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_ActionsLogging_MemoryAndDistributed_Remove_Async() => actionsLoggingMemoryAndDistributedCachingLoader.RemoveAsync(3);
    }
}