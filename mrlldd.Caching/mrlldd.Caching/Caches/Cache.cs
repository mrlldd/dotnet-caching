using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The base class for implementing caches.
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
        /// The default key suffix for given cache type.
        /// </summary>
        protected virtual string DefaultKeySuffix
        {
            get
            {
                var type = typeof(T);
                return $"{type.Namespace}.{type.Name}";
            }
        }

        /// <inheritdoc />
        public ValueTask<Result> SetAsync(T value, CancellationToken token = default)
            => PerformCachingAsync(value, DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Set(T value)
            => PerformCaching(value, DefaultKeySuffix);

        /// <inheritdoc />
        public ValueTask<Result<T>> GetAsync(CancellationToken token = default)
            => TryGetFromCacheAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result<T> Get()
            => TryGetFromCache(DefaultKeySuffix);

        /// <inheritdoc />
        public ValueTask<Result> RefreshAsync(CancellationToken token = default)
            => RefreshAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Refresh()
            => Refresh(DefaultKeySuffix);

        /// <inheritdoc />
        public ValueTask<Result> RemoveAsync(CancellationToken token = default)
            => RemoveAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Remove()
            => Remove(DefaultKeySuffix);
    }
}