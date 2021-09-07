using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class CleanCachingLoaderBenchmarks : Benchmark
    {
        private readonly ICachingLoader<short, string> cleanMemoryAndDistributedCachingLoader;
        private readonly ICachingLoader<byte, string> cleanDistributedCachingLoader;
        private readonly ICachingLoader<int, string> cleanMemoryCachingLoader;

        public CleanCachingLoaderBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCachingLoaderBenchmarks).Assembly)
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            cleanMemoryCachingLoader = cleanSp.GetRequiredService<ICachingLoader<int, string>>();
            cleanDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<byte, string>>();
            cleanMemoryAndDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<short, string>>();
        }

        [Benchmark]
        public void Loader_Clean_Memory_GetOrLoad_Sync() => cleanMemoryCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_Clean_Memory_GetOrLoad_OmitCache_Sync() => cleanMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_Clean_Memory_GetOrLoad_Async() => cleanMemoryCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_Clean_Memory_GetOrLoad_OmitCache_Async() => cleanMemoryCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_Clean_Memory_Get_Sync() => cleanMemoryCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_Clean_Memory_Get_Async() => cleanMemoryCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_Clean_Memory_Set_Sync() => cleanMemoryCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_Clean_Memory_Set_Async() => cleanMemoryCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_Clean_Memory_Refresh_Sync() => cleanMemoryCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_Clean_Memory_Refresh_Async() => cleanMemoryCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_Clean_Memory_Remove_Sync() => cleanMemoryCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_Clean_Memory_Remove_Async() => cleanMemoryCachingLoader.RemoveAsync(3);

        [Benchmark]
        public void Loader_Clean_Distributed_GetOrLoad_Sync() => cleanDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_Clean_Distributed_GetOrLoad_OmitCache_Sync() =>
            cleanDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_Clean_Distributed_GetOrLoad_Async() => cleanDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_Clean_Distributed_GetOrLoad_OmitCache_Async() =>
            cleanDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_Clean_Distributed_Get_Sync() => cleanDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_Clean_Distributed_Get_Async() => cleanDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_Clean_Distributed_Set_Sync() => cleanDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_Clean_Distributed_Set_Async() => cleanDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_Clean_Distributed_Refresh_Sync() => cleanDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_Clean_Distributed_Refresh_Async() => cleanDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_Clean_Distributed_Remove_Sync() => cleanDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_Clean_Distributed_Remove_Async() => cleanDistributedCachingLoader.RemoveAsync(3);
        
        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_GetOrLoad_Sync() => cleanMemoryAndDistributedCachingLoader.GetOrLoad(3);

        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_GetOrLoad_OmitCache_Sync() =>
            cleanMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_GetOrLoad_Async() => cleanMemoryAndDistributedCachingLoader.GetOrLoadAsync(3);

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_GetOrLoad_OmitCache_Async() =>
            cleanMemoryAndDistributedCachingLoader.GetOrLoadAsync(3, true);

        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_Get_Sync() => cleanMemoryAndDistributedCachingLoader.Get(3);

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_Get_Async() => cleanMemoryAndDistributedCachingLoader.GetAsync(3);

        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_Set_Sync() => cleanMemoryAndDistributedCachingLoader.Set(3, "3");

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_Set_Async() => cleanMemoryAndDistributedCachingLoader.SetAsync(3, "3");

        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_Refresh_Sync() => cleanMemoryAndDistributedCachingLoader.Refresh(3);

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_Refresh_Async() => cleanMemoryAndDistributedCachingLoader.RefreshAsync(3);

        [Benchmark]
        public void Loader_Clean_MemoryAndDistributed_Remove_Sync() => cleanMemoryAndDistributedCachingLoader.Remove(3);

        [Benchmark]
        public Task Loader_Clean_MemoryAndDistributed_Remove_Async() => cleanMemoryAndDistributedCachingLoader.RemoveAsync(3);
    }
}