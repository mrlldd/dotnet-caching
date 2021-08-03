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
    /// <summary>
    /// The base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Caching<T> : ICaching<T>
    {
        private IMemoryCache MemoryCache { get; set; }
        private IDistributedCache DistributedCache { get; set; }
        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<ICaching<T>> Logger { get; private set; }
        /// <summary>
        /// The options used to set up the memory cache for given object type.
        /// </summary>
        protected abstract CachingOptions MemoryCacheOptions { get; }
        /// <summary>
        /// The options used to set up the distributed cache for given object type.
        /// </summary>
        protected abstract CachingOptions DistributedCacheOptions { get; }
        /// <summary>
        /// The global cache key for given object type.
        /// </summary>
        protected abstract string CacheKey { get; }

        /// <summary>
        /// Delimiter used in cache key formatting.
        /// </summary>
        protected virtual string KeyPartsDelimiter { get; } = ":";

        /// <inheritdoc />
        public void Populate(IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ILogger<ICaching<T>> logger)
        {
            MemoryCache = memoryCache;
            DistributedCache = distributedCache;
            Logger = logger;
        }

        private string CacheKeyFactory(string suffix) 
            => string.Join(KeyPartsDelimiter,
                CacheKeyPrefixesFactory()
                    .Concat(new[]
                    {
                        CacheKey,
                        suffix
                    })
                );

        // ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// A method for storing data to cache.
        /// </summary>
        /// <param name="data">The data to be stored in cache.</param>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected internal async Task PerformCachingAsync(T data, string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            byte[] serialized = null;
            if (MemoryCacheOptions.IsCaching)
            {
                MemoryCache.Set(key, serialized = JsonSerializer.SerializeToUtf8Bytes(data), new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.Timeout
                });
                Logger.LogDebug("Put data to memory cache with key: \"{0}\".", key);
            }
            
            token.ThrowIfCancellationRequested();
            
            if (DistributedCacheOptions.IsCaching)
            {
                await DistributedCache.SetAsync(key, serialized ?? JsonSerializer.SerializeToUtf8Bytes(data), new DistributedCacheEntryOptions
                {
                    SlidingExpiration = DistributedCacheOptions.Timeout
                }, token);
                Logger.LogDebug("Put data to distributed cache with key: \"{0}\".", key);
            }
        }

        /// <summary>
        /// A method for retrieving cached data.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected async Task<T> TryGetFromCacheAsync(string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            if (MemoryCacheOptions.IsCaching && MemoryCache.TryGetValue<byte[]>(key, out var inMemory))
            {
                Logger.LogDebug("Loaded data from memory cache with key: \"{0}\".", key);
                return JsonSerializer.Deserialize<T>(inMemory);
            }

            token.ThrowIfCancellationRequested();
            if (!DistributedCacheOptions.IsCaching)
            {
                Logger.LogDebug("Not found data in memory cache with key: \"{0}\". Distributed cache is disabled.", key);
                return default;
            }

            var fromDistributed = await DistributedCache.GetAsync(key, token);

            if (fromDistributed == null || !fromDistributed.Any())
            {
                Logger.LogDebug("Not found data in both memory and distributed caches with key: \"{0}\".", key);
                return default;
            }
            
            Logger.LogDebug("Found requested data in distributed cache with key: \"{0}\".", key);
            try
            {
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Set(key, fromDistributed, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = MemoryCacheOptions.Timeout
                    });
                    Logger.LogDebug("Put requested data in memory cache with key: \"{0}\".", key);
                }
                return JsonSerializer.Deserialize<T>(fromDistributed);
            }
            catch (Exception e)
            {
                if (MemoryCacheOptions.IsCaching)
                {
                    MemoryCache.Remove(key);
                }
                Logger.LogError(e, "Failed to set data in distributed cache for key \"{0}\".", key);
                Logger.LogDebug(e, "Distributed cache data setting failed for key \"{0}\", removed memory cache entry.", key);
            }

            return default;
        }

        /// <summary>
        /// The method used for creating additional global cache keys prefixes in order to make them more unique.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> of prefixes.</returns>
        protected abstract IEnumerable<string> CacheKeyPrefixesFactory();
    }
}