using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result.Extensions;

namespace mrlldd.Caching.Caches
{
    public sealed class Cache
    {
        private readonly ICacheProvider cacheProvider;
        private readonly ICacheOptions defaultCacheOptions;

        internal Cache(ICacheProvider cacheProvider, ICacheOptions defaultCacheOptions)
        {
            this.cacheProvider = cacheProvider;
            this.defaultCacheOptions = defaultCacheOptions;
        }

        public Task SetAsync<T>(T value, CancellationToken token = default)
            => GetCache<T>().SetAsync(value, token);

        public void Set<T>(T value, CancellationToken token = default)
            => GetCache<T>().SetAsync(value, token);

        public Task<T?> GetAsync<T>(CancellationToken token = default)
            => GetCache<T>().GetAsync(token);

        public T? Get<T>(CancellationToken token = default)
            => GetCache<T>().Get(token);

        public Task RefreshAsync<T>(CancellationToken token = default)
            => GetCache<T>().RefreshAsync(token);

        public void Refresh<T>(CancellationToken token = default)
            => GetCache<T>().Refresh(token);

        public Task RemoveAsync<T>(CancellationToken token = default)
            => GetCache<T>().RemoveAsync(token);

        public void Remove<T>(CancellationToken token = default)
            => GetCache<T>().Remove(token);

        private ICache<T> GetCache<T>()
            => cacheProvider.Get<T>()
                .Map(x => x.Successful
                    ? x.UnwrapAsSuccess()
                    : new DefaultCache<T>(defaultCacheOptions)
                );
    }

    /// <summary>
    /// The base class for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Cache<T> : Caching<T>, ICache<T>
    {
        /// <inheritdoc />
        protected sealed override IEnumerable<string> CacheKeyPrefixesFactory()
        {
            yield break;
        }

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// The default key suffix for given cache type.
        /// </summary>
        protected virtual string DefaultKeySuffix { get; } = typeof(T).Map(x => $"{x.Namespace}.{x.Name}");

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