using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;

namespace mrlldd.Caching
{
    /// <summary>
    /// The base class for implementing caching utilities.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TStoreFlag">The caching store flag.</typeparam>
    public abstract class Caching<T, TStoreFlag> : ICaching 
        where TStoreFlag : CachingFlag
    {
        private IStoreOperationProvider StoreOperationProvider { get; set; } = null!;

        private ICacheStore<TStoreFlag> Store { get; set; } = null!;

        /// <summary>
        /// The options used to set up the cache for given object type.
        /// </summary>
        protected abstract CachingOptions Options { get; }

        /// <summary>
        /// The global cache key for given object type.
        /// </summary>
        protected abstract string CacheKey { get; }

        /// <summary>
        /// Delimiter used in cache key formatting.
        /// </summary>
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual string CacheKeyDelimiter => ":";

        /// <inheritdoc />
        public void Populate(IServiceProvider serviceProvider,
            IStoreOperationProvider storeOperationProvider)
        {
            Store = serviceProvider
                .GetRequiredService<ICacheStoreProvider<TStoreFlag>>()
                .CacheStore;
            StoreOperationProvider = storeOperationProvider;
        }

        private string CacheKeyFactory(string suffix)
            => string.Join(CacheKeyDelimiter, CacheKeyPrefix, CacheKey, suffix);

        // ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// A method for storing data to cache.
        /// </summary>
        /// <param name="data">The data to be stored in cache.</param>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        protected internal Result PerformCaching(T? data, string keySuffix)
        {
            if (!Options.IsCaching)
            {
                return new DisabledCachingException();
            }

            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.Set(key, data, Options, operation);
        }

        // ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// A method for storing data to cache.
        /// </summary>
        /// <param name="data">The data to be stored in cache.</param>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected internal Task<Result> PerformCachingAsync(T? data, string keySuffix,
            CancellationToken token = default)
        {
            if (!Options.IsCaching)
            {
                return Task.FromResult<Result>(new DisabledCachingException());
            }

            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.SetAsync(key, data, Options, operation, token);
        }

        /// <summary>
        /// A method for retrieving cached data entry.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected Result<T?> TryGetFromCache(string keySuffix)
        {
            if (!Options.IsCaching)
            {
                return new DisabledCachingException();
            }

            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.Get<T>(key, operation);
        }

        /// <summary>
        /// A method for retrieving cached data entry.
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns data or null.</returns>
        protected Task<Result<T?>> TryGetFromCacheAsync(string keySuffix, CancellationToken token = default)
        {
            if (!Options.IsCaching)
            {
                return Task.FromResult<Result<T?>>(new DisabledCachingException());
            }

            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.GetAsync<T>(key, operation, token);
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        protected Result Refresh(string keySuffix)
        {
            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);

            return Store.Refresh(key, operation);
        }

        /// <summary>
        /// A method for refreshing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected Task<Result> RefreshAsync(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.RefreshAsync(key, operation, token);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        protected Result Remove(string keySuffix)
        {
            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);

            return Store.Remove(key, operation);
        }

        /// <summary>
        /// A method for removing cached data entry expiration. 
        /// </summary>
        /// <param name="keySuffix">The suffix extension to generated cache key.</param>
        /// <param name="token">The cancellation token.</param>
        protected Task<Result> RemoveAsync(string keySuffix, CancellationToken token)
        {
            var operation = StoreOperationProvider.Next(CacheKeyDelimiter);
            var key = CacheKeyFactory(keySuffix);
            return Store.RemoveAsync(key, operation, token);
        }

        /// <summary>
        /// Global cache key prefix.
        /// </summary>
        /// <returns>The string.</returns>
        protected abstract string CacheKeyPrefix { get; }
    }
}