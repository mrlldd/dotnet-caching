using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching
{
    /// <summary>
    /// The base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Caching<T> : ICaching
    {
        private IStoreOperationProvider StoreOperationProvider { get; set; } = null!;
        private IMemoryCacheStore MemoryCacheStore { get; set; } = null!;
        private IDistributedCacheStore DistributedCacheStore { get; set; } = null!;

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
        public void Populate(IMemoryCacheStore memoryCacheStore,
            IDistributedCacheStore distributedCacheStore,
            IStoreOperationProvider storeOperationProvider)
        {
            MemoryCacheStore = memoryCacheStore;
            DistributedCacheStore = distributedCacheStore;
            StoreOperationProvider = storeOperationProvider;
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

            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);
            MemoryCacheStore.Set(key, data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = MemoryCacheOptions.SlidingExpiration
            }, operation);

            token.ThrowIfCancellationRequested();

            DistributedCacheStore.Set(key, data, new DistributedCacheEntryOptions
            {
                SlidingExpiration = DistributedCacheOptions.SlidingExpiration
            }, operation);
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

            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);
            await MemoryCacheStore.SetAsync(key, data, new MemoryCacheEntryOptions
            {
                SlidingExpiration = MemoryCacheOptions.SlidingExpiration
            }, operation, token);

            token.ThrowIfCancellationRequested();

            await DistributedCacheStore.SetAsync(key, data, new DistributedCacheEntryOptions
            {
                SlidingExpiration = DistributedCacheOptions.SlidingExpiration
            }, operation, token);
        }

        /// <summary>
        /// A method for retrieving cached data entry.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected T? TryGetFromCache(string keySuffix, CancellationToken token = default)
        {
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);
            var fromMemory = MemoryCacheStore.Get<T>(key, operation);
            if (fromMemory.Successful)
            {
                var unwrapped = fromMemory.UnwrapAsSuccess();
                if (unwrapped != null)
                {
                    return fromMemory;
                }
            }

            token.ThrowIfCancellationRequested();

            var fromDistributed = DistributedCacheStore.Get<T>(key, operation);

            token.ThrowIfCancellationRequested();

            if (!fromDistributed.Successful)
            {
                MemoryCacheStore.Remove(key, operation);
                return default;
            }

            token.ThrowIfCancellationRequested();

            var fromDistributedUnwrapped = fromDistributed.UnwrapAsSuccess();

            if (fromDistributedUnwrapped == null)
            {
                return default;
            }

            MemoryCacheStore.Set(key, fromDistributedUnwrapped,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.SlidingExpiration
                }, operation);

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
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);
            var fromMemory = await MemoryCacheStore.GetAsync<T>(key, operation, token);
            if (fromMemory.Successful)
            {
                var unwrapped = fromMemory.UnwrapAsSuccess();
                if (unwrapped != null)
                {
                    return fromMemory;
                }
            }

            token.ThrowIfCancellationRequested();

            var fromDistributed = await DistributedCacheStore.GetAsync<T>(key,operation, token);

            token.ThrowIfCancellationRequested();

            if (!fromDistributed.Successful)
            {
                await MemoryCacheStore.RemoveAsync(key, operation, token);
                return default;
            }

            token.ThrowIfCancellationRequested();

            var fromDistributedUnwrapped = fromDistributed.UnwrapAsSuccess();

            if (fromDistributedUnwrapped == null)
            {
                return default;
            }

            await MemoryCacheStore.SetAsync(key, fromDistributedUnwrapped,
                new MemoryCacheEntryOptions
                {
                    SlidingExpiration = MemoryCacheOptions.SlidingExpiration
                }, operation,token);

            return fromDistributedUnwrapped;
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected void Refresh(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);

            MemoryCacheStore.Refresh(key, operation);

            token.ThrowIfCancellationRequested();

            DistributedCacheStore.Refresh(key, operation);
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected async Task RefreshAsync(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);

            await MemoryCacheStore.RefreshAsync(key, operation, token);

            token.ThrowIfCancellationRequested();

            await DistributedCacheStore.RefreshAsync(key, operation, token);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected void Remove(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);

            MemoryCacheStore.Remove(key, operation);

            token.ThrowIfCancellationRequested();

            DistributedCacheStore.Remove(key, operation);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected async Task RemoveAsync(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next();
            var key = CacheKeyFactory(keySuffix);

            await MemoryCacheStore.RemoveAsync(key, operation, token);

            token.ThrowIfCancellationRequested();

            await DistributedCacheStore.RemoveAsync(keySuffix, operation, token);
        }

        /// <summary>
        /// The method used for creating additional global cache keys prefixes in order to make them more unique.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> of prefixes.</returns>
        protected abstract IEnumerable<string> CacheKeyPrefixesFactory();
    }
}