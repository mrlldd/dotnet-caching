using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caching
{
    /// <summary>
    /// The base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Caching<T> : ICaching<T>
    {
        private IMemoryCachingStore MemoryCachingStore { get; set; }
        private IDistributedCachingStore DistributedCachingStore { get; set; }

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
        public void Populate(IMemoryCachingStore memoryCachingCache,
            IDistributedCachingStore distributedCachingCache,
            ILogger<ICaching<T>> logger)
        {
            MemoryCachingStore = memoryCachingCache;
            DistributedCachingStore = distributedCachingCache;
            Logger = logger;
        }

        /// <inheritdoc />
        public bool IsUsingMemory => MemoryCacheOptions.IsCaching;

        /// <inheritdoc />
        public bool IsUsingDistributed => DistributedCacheOptions.IsCaching;

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
            if (!(IsUsingDistributed || IsUsingMemory))
            {
                return;
            }

            var memorySavingResult = await MemoryCachingStore.SetAsync(key, data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = MemoryCacheOptions.SlidingExpiration
            }, token);
            if (memorySavingResult.Successful)
            {
                Logger.LogDebug("Put data to memory cache with key: \"{0}\".", key);
            }
            else
            {
                Logger.LogError(memorySavingResult, "Failed to put data to memory cache with key: \"{0}\".", key);
            }

            token.ThrowIfCancellationRequested();

            var distributedSavingResult = await DistributedCachingStore.SetAsync(key, data, new DistributedCacheEntryOptions
            {
                SlidingExpiration = DistributedCacheOptions.SlidingExpiration
            }, token);
            if (distributedSavingResult.Successful)
            {
                Logger.LogDebug("Put data to distributed cache with key: \"{0}\".", key);
            }
            else
            {
                Logger.LogError(distributedSavingResult, "Failed to put data to distributed cache with key: \"{0}\".", key);
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
            var fromMemory = await MemoryCachingStore.GetAsync<T>(key, token);
            if (fromMemory.Successful)
            {
                var unwrapped = fromMemory.UnwrapAsSuccess();
                if (unwrapped != null)
                {
                    Logger.LogDebug("Loaded data from memory cache with key: \"{0}\".", key);
                    return fromMemory;
                }
            }
            else
            {
                Logger.LogDebug("Entry with key \"{0}\" not found in memory cache.", key);
            }

            token.ThrowIfCancellationRequested();

            var fromDistributed = await DistributedCachingStore.GetAsync<T>(key, token);

            if (!fromDistributed.Successful)
            {
                Logger.LogError(fromDistributed, "Failed to get entry with key: \"{0}\" from distributed cache.", key);
                var memoryRemovingResult = await MemoryCachingStore.RemoveAsync(key, token);
                if (memoryRemovingResult.Successful)
                {
                    Logger.LogDebug("Removed entry with key \"{0}\" from memory cache.", key);
                }
                else
                {
                    Logger.LogError(memoryRemovingResult, "Failed to remove entry with key \"{0}\" from memory cache.",
                        key);
                }
                return default;
            }

            var fromDistributedUnwrapped = fromDistributed.UnwrapAsSuccess();

            if (fromDistributedUnwrapped == null)
            {
                Logger.LogDebug("Entry with key \"{0}\" not found in distributed cache.", key);
                return default;
            }

            Logger.LogDebug("Found requested data in distributed cache with key: \"{0}\".", key);
            var memorySavingResult = await MemoryCachingStore.SetAsync(key, fromDistributedUnwrapped,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.SlidingExpiration
                }, token);

            if (!memorySavingResult.Successful)
            {
                Logger.LogError(memorySavingResult, "Failed to set entry with key \"{0}\" to memory cache.", key);
            }

            return fromDistributedUnwrapped;
        }

        /// <summary>
        /// The method used for creating additional global cache keys prefixes in order to make them more unique.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> of prefixes.</returns>
        protected abstract IEnumerable<string> CacheKeyPrefixesFactory();
    }
}