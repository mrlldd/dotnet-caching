using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    ///     The base class for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TStoreFlag">The cache store flag type.</typeparam>
    public abstract class Cache<T, TStoreFlag> : Caching<T, TStoreFlag>,
        ICache<T, TStoreFlag>,
        IInternalCache<T, TStoreFlag>
        where TStoreFlag : CachingFlag
    {
        /// <inheritdoc />
        protected sealed override string CacheKeyPrefix => "cache";

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        ///     The cache key suffix for given cache type.
        /// </summary>
        protected virtual string CacheKeySuffix
        {
            get
            {
                var type = typeof(T);
                return $"{type.Namespace}.{type.Name}";
            }
        }

        /// <inheritdoc />
        public ValueTask<Result> SetAsync(T value, CancellationToken token = default)
        {
            return PerformCachingAsync(value, CacheKeySuffix, token);
        }

        /// <inheritdoc />
        public Result Set(T value)
        {
            return PerformCaching(value, CacheKeySuffix);
        }

        /// <inheritdoc />
        public ValueTask<Result<T>> GetAsync(CancellationToken token = default)
        {
            return TryGetFromCacheAsync(CacheKeySuffix, token);
        }

        /// <inheritdoc />
        public Result<T> Get()
        {
            return TryGetFromCache(CacheKeySuffix);
        }

        /// <inheritdoc />
        public ValueTask<Result> RefreshAsync(CancellationToken token = default)
        {
            return RefreshAsync(CacheKeySuffix, token);
        }

        /// <inheritdoc />
        public Result Refresh()
        {
            return Refresh(CacheKeySuffix);
        }

        /// <inheritdoc />
        public ValueTask<Result> RemoveAsync(CancellationToken token = default)
        {
            return RemoveAsync(CacheKeySuffix, token);
        }

        /// <inheritdoc />
        public Result Remove()
        {
            return Remove(CacheKeySuffix);
        }

        /// <inheritdoc />
        protected sealed override void EnrichWithDependencies(IServiceProvider serviceProvider)
        {
            base.EnrichWithDependencies(serviceProvider);
        }
    }
}