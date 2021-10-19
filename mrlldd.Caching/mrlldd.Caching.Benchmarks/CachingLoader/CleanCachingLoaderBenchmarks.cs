using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;

namespace mrlldd.Caching.Benchmarks.CachingLoader
{
    public class CleanCachingLoaderBenchmarks : Benchmark
    {
        private readonly ICachingLoader<byte, string, InDistributed> cleanDistributedCachingLoader;
        private readonly ICachingLoader<int, string, InMemory> cleanMemoryCachingLoader;

        public CleanCachingLoaderBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddDistributedMemoryCache()
                .AddCaching(typeof(CleanCachingLoaderBenchmarks).Assembly)
                .BuildServiceProvider()
                .CreateScope().ServiceProvider;
            cleanMemoryCachingLoader = cleanSp.GetRequiredService<ICachingLoader<int, string, InMemory>>();
            cleanDistributedCachingLoader = cleanSp.GetRequiredService<ICachingLoader<byte, string, InDistributed>>();
        }

        [Benchmark]
        public void Loader_Clean_Memory_GetOrLoad_Sync()
        {
            cleanMemoryCachingLoader.GetOrLoad(3);
        }

        [Benchmark]
        public void Loader_Clean_Memory_GetOrLoad_OmitCache_Sync()
        {
            cleanMemoryCachingLoader.GetOrLoadAsync(3, true);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Memory_GetOrLoad_Async()
        {
            return cleanMemoryCachingLoader.GetOrLoadAsync(3);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Memory_GetOrLoad_OmitCache_Async()
        {
            return cleanMemoryCachingLoader.GetOrLoadAsync(3, true);
        }

        [Benchmark]
        public void Loader_Clean_Memory_Get_Sync()
        {
            cleanMemoryCachingLoader.Get(3);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Memory_Get_Async()
        {
            return cleanMemoryCachingLoader.GetAsync(3);
        }

        [Benchmark]
        public void Loader_Clean_Memory_Set_Sync()
        {
            cleanMemoryCachingLoader.Set(3, "3");
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Memory_Set_Async()
        {
            return cleanMemoryCachingLoader.SetAsync(3, "3");
        }

        [Benchmark]
        public void Loader_Clean_Memory_Refresh_Sync()
        {
            cleanMemoryCachingLoader.Refresh(3);
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Memory_Refresh_Async()
        {
            return cleanMemoryCachingLoader.RefreshAsync(3);
        }

        [Benchmark]
        public void Loader_Clean_Memory_Remove_Sync()
        {
            cleanMemoryCachingLoader.Remove(3);
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Memory_Remove_Async()
        {
            return cleanMemoryCachingLoader.RemoveAsync(3);
        }

        [Benchmark]
        public void Loader_Clean_Distributed_GetOrLoad_Sync()
        {
            cleanDistributedCachingLoader.GetOrLoad(3);
        }

        [Benchmark]
        public void Loader_Clean_Distributed_GetOrLoad_OmitCache_Sync()
        {
            cleanDistributedCachingLoader.GetOrLoadAsync(3, true);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Distributed_GetOrLoad_Async()
        {
            return cleanDistributedCachingLoader.GetOrLoadAsync(3);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Distributed_GetOrLoad_OmitCache_Async()
        {
            return cleanDistributedCachingLoader.GetOrLoadAsync(3, true);
        }

        [Benchmark]
        public void Loader_Clean_Distributed_Get_Sync()
        {
            cleanDistributedCachingLoader.Get(3);
        }

        [Benchmark]
        public ValueTask<Result<string>> Loader_Clean_Distributed_Get_Async()
        {
            return cleanDistributedCachingLoader.GetAsync(3);
        }

        [Benchmark]
        public void Loader_Clean_Distributed_Set_Sync()
        {
            cleanDistributedCachingLoader.Set(3, "3");
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Distributed_Set_Async()
        {
            return cleanDistributedCachingLoader.SetAsync(3, "3");
        }

        [Benchmark]
        public void Loader_Clean_Distributed_Refresh_Sync()
        {
            cleanDistributedCachingLoader.Refresh(3);
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Distributed_Refresh_Async()
        {
            return cleanDistributedCachingLoader.RefreshAsync(3);
        }

        [Benchmark]
        public void Loader_Clean_Distributed_Remove_Sync()
        {
            cleanDistributedCachingLoader.Remove(3);
        }

        [Benchmark]
        public ValueTask<Result> Loader_Clean_Distributed_Remove_Async()
        {
            return cleanDistributedCachingLoader.RemoveAsync(3);
        }
    }
}