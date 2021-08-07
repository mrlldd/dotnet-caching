using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caching
{
    /// <summary>
    /// The base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Caching<T> : ICaching
    {
        private IMemoryCachingStore MemoryCachingStore { get; set; } = null!;
        private IDistributedCachingStore DistributedCachingStore { get; set; } = null!;

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
        protected virtual string KeyPartsDelimiter => ":";

        /// <inheritdoc />
        public void Populate(IMemoryCachingStore memoryCachingStore,
            IDistributedCachingStore distributedCachingStore)
        {
            MemoryCachingStore = memoryCachingStore;
            DistributedCachingStore = distributedCachingStore;
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
        protected internal void PerformCaching(T? data, string keySuffix, CancellationToken token = default)
        {
            if (data == null)
            {
                return;
            }

            if (!(IsUsingDistributed || IsUsingMemory))
            {
                return;
            }

            var key = CacheKeyFactory(keySuffix);
            MemoryCachingStore.Set(key, data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = MemoryCacheOptions.SlidingExpiration
            });

            token.ThrowIfCancellationRequested();

            DistributedCachingStore.Set(key, data, new DistributedCacheEntryOptions
            {
                SlidingExpiration = DistributedCacheOptions.SlidingExpiration
            });
        }

        // ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// A method for storing data to cache.
        /// </summary>
        /// <param name="data">The data to be stored in cache.</param>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected internal async Task PerformCachingAsync(T? data, string keySuffix, CancellationToken token = default)
        {
            if (data == null)
            {
                return;
            }
            
            if (!(IsUsingDistributed || IsUsingMemory))
            {
                return;
            }

            var key = CacheKeyFactory(keySuffix);
            await MemoryCachingStore.SetAsync(key, data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = MemoryCacheOptions.SlidingExpiration
            }, token);

            token.ThrowIfCancellationRequested();

            await DistributedCachingStore.SetAsync(key, data, new DistributedCacheEntryOptions
            {
                SlidingExpiration = DistributedCacheOptions.SlidingExpiration
            }, token);
        }

        /// <summary>
        /// A method for retrieving cached data entry.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected T? TryGetFromCache(string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            var fromMemory = MemoryCachingStore.Get<T>(key);
            if (fromMemory.Successful)
            {
                var unwrapped = fromMemory.UnwrapAsSuccess();
                if (unwrapped != null)
                {
                    return fromMemory;
                }
            }

            token.ThrowIfCancellationRequested();

            var fromDistributed = DistributedCachingStore.Get<T>(key);

            token.ThrowIfCancellationRequested();

            if (!fromDistributed.Successful)
            {
                MemoryCachingStore.Remove(key);
                return default;
            }

            token.ThrowIfCancellationRequested();

            var fromDistributedUnwrapped = fromDistributed.UnwrapAsSuccess();

            if (fromDistributedUnwrapped == null)
            {
                return default;
            }

            MemoryCachingStore.Set(key, fromDistributedUnwrapped,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.SlidingExpiration
                });

            return fromDistributedUnwrapped;
        }

        /// <summary>
        /// A method for retrieving cached data entry.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected async Task<T?> TryGetFromCacheAsync(string keySuffix, CancellationToken token = default)
        {
            var key = CacheKeyFactory(keySuffix);
            var fromMemory = await MemoryCachingStore.GetAsync<T>(key, token);
            if (fromMemory.Successful)
            {
                var unwrapped = fromMemory.UnwrapAsSuccess();
                if (unwrapped != null)
                {
                    return fromMemory;
                }
            }

            token.ThrowIfCancellationRequested();

            var fromDistributed = await DistributedCachingStore.GetAsync<T>(key, token);

            token.ThrowIfCancellationRequested();

            if (!fromDistributed.Successful)
            {
                await MemoryCachingStore.RemoveAsync(key, token);
                return default;
            }

            token.ThrowIfCancellationRequested();

            var fromDistributedUnwrapped = fromDistributed.UnwrapAsSuccess();

            if (fromDistributedUnwrapped == null)
            {
                return default;
            }

            await MemoryCachingStore.SetAsync(key, fromDistributedUnwrapped,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.SlidingExpiration
                }, token);

            return fromDistributedUnwrapped;
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected void Refresh(string keySuffix, CancellationToken token)
        {
            var key = CacheKeyFactory(keySuffix);

            MemoryCachingStore.Refresh(key);

            token.ThrowIfCancellationRequested();

            DistributedCachingStore.Refresh(key);
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected async Task RefreshAsync(string keySuffix, CancellationToken token)
        {
            var key = CacheKeyFactory(keySuffix);

            await MemoryCachingStore.RefreshAsync(key, token);

            token.ThrowIfCancellationRequested();

            await DistributedCachingStore.RefreshAsync(key, token);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected void Remove(string keySuffix, CancellationToken token)
        {
            var key = CacheKeyFactory(keySuffix);

            MemoryCachingStore.Remove(key);

            token.ThrowIfCancellationRequested();

            DistributedCachingStore.Remove(key);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected async Task RemoveAsync(string keySuffix, CancellationToken token)
        {
            var key = CacheKeyFactory(keySuffix);

            await MemoryCachingStore.RemoveAsync(key, token);

            token.ThrowIfCancellationRequested();

            await DistributedCachingStore.RemoveAsync(keySuffix, token);
        }

        /// <summary>
        /// The method used for creating additional global cache keys prefixes in order to make them more unique.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> of prefixes.</returns>
        protected abstract IEnumerable<string> CacheKeyPrefixesFactory();
    }
}