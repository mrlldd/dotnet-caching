using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace mrlldd.Caching.Caching
{
    public abstract class Caching<T> : ICaching<T>
    {
        private IMemoryCache MemoryCache { get; set; }
        private IDistributedCache DistributedCache { get; set; }
        protected ILogger<ICaching<T>> Logger { get; private set; }
        protected abstract CachingOptions MemoryCacheOptions { get; }
        protected abstract CachingOptions DistributedCacheOptions { get; }
        protected abstract string CacheKey { get; }

        public void Populate(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ILogger<ICaching<T>> logger)
        {
            MemoryCache = memoryCache;
            DistributedCache = distributedCache;
            Logger = logger;
        }

        private string CacheKeyFactory(string suffix) 
            => string.Join(":",
                CacheKeyPrefixesFactory()
                    .Concat(new[]
                    {
                        CacheKey,
                        suffix
                    })
                );

        // ReSharper disable once MemberCanBeProtected.Global
        protected internal async Task PerformCachingAsync(T data, string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            if (MemoryCacheOptions.IsCaching)
            {
                MemoryCache.Set(key, data, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.Timeout
                });
                Logger
                    .LogDebug("Put data to memory cache with key: {0}", key);
            }
            
            token.ThrowIfCancellationRequested();
            
            if (DistributedCacheOptions.IsCaching)
            {
                await DistributedCache.SetStringAsync(key, JsonSerializer.Serialize(data), new DistributedCacheEntryOptions
                {
                    SlidingExpiration = DistributedCacheOptions.Timeout
                }, token);
                Logger
                    .LogDebug("Put data to distributed cache with key: {0}", key);
            }
        }

        protected async Task<T> TryGetFromCacheAsync(string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            if (MemoryCacheOptions.IsCaching && MemoryCache.TryGetValue<T>(key, out var inMemory))
            {
                Logger
                    .LogDebug("Loaded data from memory cache with key {0}.", key);
                return inMemory;
            }

            token.ThrowIfCancellationRequested();
            if (!DistributedCacheOptions.IsCaching)
            {
                Logger
                    .LogDebug("Not found data in memory cache with key: {0}. Distributed cache is disabled.", key);
                return default;
            }

            var inDistributed = await DistributedCache.GetStringAsync(key, token);

            if (string.IsNullOrEmpty(inDistributed))
            {
                Logger
                    .LogDebug("Not found data in both memory and distributed caches with key: {0}.", key);
                return default;
            }
            
            Logger
                .LogDebug("Found requested data in distributed cache with key: {0}.", key);
            try
            {
                var deserialized = JsonSerializer.Deserialize<T>(inDistributed);
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Set(key, deserialized, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = MemoryCacheOptions.Timeout
                    });
                    Logger
                        .LogDebug("Put requested data in memory cache with key: {0}.", key);
                }
                return deserialized;
            }
            catch (Exception e)
            {
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Remove(key);
                }
                Logger
                    .LogDebug(e, "Cache data load failed for key {0}, removed memory cache value.", key);
            }

            return default;
        }

        protected abstract IEnumerable<string> CacheKeyPrefixesFactory();
    }
}