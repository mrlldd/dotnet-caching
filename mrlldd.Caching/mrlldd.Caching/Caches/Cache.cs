using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result.Extensions;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The cache service-wrapper that provides generic access to generic caches.
    /// </summary>
    public sealed class Cache
    {
        private readonly ICacheProvider cacheProvider;

        /// <summary>
        /// The constructor for cache.
        /// </summary>
        /// <param name="cacheProvider"> The cache provider.</param>
        public Cache(ICacheProvider cacheProvider) 
            => this.cacheProvider = cacheProvider;

        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task SetAsync<T>(T value, CancellationToken token = default)
            => GetCache<T>().SetAsync(value, token);

        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        public void Set<T>(T value, CancellationToken token = default)
            => GetCache<T>().SetAsync(value, token);

        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task{TResult}"/> that returns <typeparamref name="T"/>.</returns>
        public Task<T?> GetAsync<T>(CancellationToken token = default)
            => GetCache<T>().GetAsync(token);

        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The value of type <typeparamref name="T"/>.</returns>
        public T? Get<T>(CancellationToken token = default)
            => GetCache<T>().Get(token);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task RefreshAsync<T>(CancellationToken token = default)
            => GetCache<T>().RefreshAsync(token);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        public void Refresh<T>(CancellationToken token = default)
            => GetCache<T>().Refresh(token);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task RemoveAsync<T>(CancellationToken token = default)
            => GetCache<T>().RemoveAsync(token);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        public void Remove<T>(CancellationToken token = default)
            => GetCache<T>().Remove(token);

        private ICache<T> GetCache<T>()
            => cacheProvider.GetOrDefault<T>();
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