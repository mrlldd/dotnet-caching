using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public interface ICache<T> : ICaching
    {
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        Task SetAsync(T value, CancellationToken token = default);

        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        public void Set(T value, CancellationToken token = default);

        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Task<T?> GetAsync(CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        public T? Get(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task RefreshAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        void Refresh(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task RemoveAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        void Remove(CancellationToken token = default);
    }
} 