﻿using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace mrlldd.Caching.Benchmarks.Cache
{
    [MemoryDiagnoser]
    [ThreadingDiagnoser]
    public class MicrosoftCacheBenchmarks
    {
        private readonly IMemoryCache microsoftMemoryCache;
        private readonly MemoryCacheEntryOptions microsoftMemoryEntryOptions;
        private readonly IDistributedCache microsoftDistributedMemoryCache;
        private readonly DistributedCacheEntryOptions microsoftDistributedEntryOptions;

        public MicrosoftCacheBenchmarks()
        {
            var cleanSp = new ServiceCollection()
                .AddMemoryCache()
                .AddDistributedMemoryCache()
                .BuildServiceProvider();
                
            microsoftMemoryCache = cleanSp.GetRequiredService<IMemoryCache>();
            microsoftMemoryEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };

            microsoftDistributedMemoryCache = cleanSp.GetRequiredService<IDistributedCache>();
            microsoftDistributedEntryOptions = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }
        [Benchmark]
        public void Cache_Microsoft_Memory_Set_Sync() => microsoftMemoryCache.Set("key", 3, microsoftMemoryEntryOptions);

        [Benchmark]
        public Task Cache_Microsoft_Memory_Set_Async()
        {
            microsoftMemoryCache.Set("key", 3, microsoftMemoryEntryOptions);
            return Task.CompletedTask;
        }
        
        [Benchmark]
        public void Cache_Microsoft_Memory_Get_Sync() => microsoftMemoryCache.Get<int>("key");

        [Benchmark]
        public Task Cache_Microsoft_Memory_Get_Async()
        {
            microsoftMemoryCache.Get<int>("key");
            return Task.CompletedTask;
        }
        
        [Benchmark]
        public void Cache_Microsoft_Memory_Remove_Sync() => microsoftMemoryCache.Remove("key");

        [Benchmark]
        public Task Cache_Microsoft_Memory_Remove_Async()
        {
            microsoftMemoryCache.Remove("key");
            return Task.CompletedTask;
        }
        
        [Benchmark]
        public void Cache_Microsoft_DistributedMemory_Set_Sync() => microsoftDistributedMemoryCache.Set("key", BitConverter.GetBytes(3), microsoftDistributedEntryOptions);

        [Benchmark]
        public Task Cache_Microsoft_DistributedMemory_Set_Async() =>
            microsoftDistributedMemoryCache.SetAsync("key", BitConverter.GetBytes(3), microsoftDistributedEntryOptions);
        
        [Benchmark]
        public void Cache_Microsoft_DistributedMemory_Get_Sync() => microsoftDistributedMemoryCache.Get("key");

        [Benchmark]
        public Task Cache_Microsoft_DistributedMemory_Get_Async() => microsoftDistributedMemoryCache.GetAsync("key");
        
        [Benchmark]
        public void Cache_Microsoft_DistributedMemory_Refresh_Sync() => microsoftDistributedMemoryCache.Refresh("key");

        [Benchmark]
        public Task Cache_Microsoft_DistributedMemory_Refresh_Async() => microsoftDistributedMemoryCache.RefreshAsync("key");
        
        [Benchmark]
        public void Cache_Microsoft_DistributedMemory_Remove_Sync() => microsoftDistributedMemoryCache.Remove("key");

        [Benchmark]
        public Task Cache_Microsoft_DistributedMemory_Remove_Async() => microsoftDistributedMemoryCache.RemoveAsync("key");
    }
}