using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The base class for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Cache<T> : Caching<T>, ICache<T>
    {
        /// <inheritdoc />
        protected override IEnumerable<string> CacheKeyPrefixesFactory()
        {
            yield break;
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// The default key suffix for given cache type.
        /// </summary>
        protected virtual string DefaultKeySuffix { get; } = typeof(T).ToString();

        /// <inheritdoc />
        public Task SetAsync(T value, CancellationToken token = default)
            => PerformCachingAsync(value, DefaultKeySuffix, token);

        /// <inheritdoc />
        public void Set(T value, CancellationToken token = default)
            => PerformCaching(value, DefaultKeySuffix, token);

        /// <inheritdoc />
        public Task<T?> GetAsync(CancellationToken token = default)
            => TryGetFromCacheAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public T? Get(CancellationToken token = default)
            => TryGetFromCache(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Task RefreshAsync(CancellationToken token = default)
            => RefreshAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public void Refresh(CancellationToken token = default)
            => Refresh(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Task RemoveAsync(CancellationToken token = default)
            => RemoveAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public void Remove(CancellationToken token = default)
            => Remove(DefaultKeySuffix, token);
    }
}