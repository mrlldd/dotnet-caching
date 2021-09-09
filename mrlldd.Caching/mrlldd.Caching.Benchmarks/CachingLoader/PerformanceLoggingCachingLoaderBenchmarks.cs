using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class PerformanceLoggingCachingLoaderBenchmarks : Benchmark
    {
        private readonly ICachingLoader<short, string> perfLoggingMemoryAndDistributedCachingLoader;
        private readonly ICachingLoader<byte, string> perfLoggingDistributedCachingLoader;
        private readonly ICachingLoader<int, string> perfLoggingMemoryCachingLoader;

        public PerformanceLoggingCachingLoaderBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCachingLoaderBenchmarks).Assembly)
                .WithPerformanceLogging()
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            perfLoggingMemoryCachingLoader = cleanSp.GetRequiredService<ICachingLoader<int, string>>();
            perfLoggingDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<byte, string>>();
            perfLoggingMemoryAndDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<short, string>>();
        }

        [Benchmark]
        public void Loader_PerfLogging_Memory_GetOrLoad_Sync() => perfLoggingMemoryCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_PerfLogging_Memory_GetOrLoad_OmitCache_Sync() =>
            perfLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_PerfLogging_Memory_GetOrLoad_Async() => perfLoggingMemoryCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_PerfLogging_Memory_GetOrLoad_OmitCache_Async() =>
            perfLoggingMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_PerfLogging_Memory_Get_Sync() => perfLoggingMemoryCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_PerfLogging_Memory_Get_Async() => perfLoggingMemoryCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_Memory_Set_Sync() => perfLoggingMemoryCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_PerfLogging_Memory_Set_Async() => perfLoggingMemoryCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_PerfLogging_Memory_Refresh_Sync() => perfLoggingMemoryCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_PerfLogging_Memory_Refresh_Async() => perfLoggingMemoryCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_Memory_Remove_Sync() => perfLoggingMemoryCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_PerfLogging_Memory_Remove_Async() => perfLoggingMemoryCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_Distributed_GetOrLoad_Sync() => perfLoggingDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_PerfLogging_Distributed_GetOrLoad_OmitCache_Sync() =>
            perfLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_GetOrLoad_Async() =>
            perfLoggingDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_GetOrLoad_OmitCache_Async() =>
            perfLoggingDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_PerfLogging_Distributed_Get_Sync() => perfLoggingDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_Get_Async() => perfLoggingDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_Distributed_Set_Sync() => perfLoggingDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_Set_Async() => perfLoggingDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_PerfLogging_Distributed_Refresh_Sync() => perfLoggingDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_Refresh_Async() =>
            perfLoggingDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_Distributed_Remove_Sync() => perfLoggingDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_PerfLogging_Distributed_Remove_Async() => perfLoggingDistributedCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_GetOrLoad_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_GetOrLoad_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_GetOrLoad_OmitCache_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_Get_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_Get_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_Set_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_Set_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_Refresh_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_Refresh_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_PerfLogging_MemoryAndDistributed_Remove_Sync() =>
            perfLoggingMemoryAndDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_PerfLogging_MemoryAndDistributed_Remove_Async() =>
            perfLoggingMemoryAndDistributedCachingLoader.RemoveAsync(3);
    }
}