using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The interface that represents cache service-wrapper
    /// that provides generic access to generic caches.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SetAsync<T>(T value, CancellationToken token = default);
        
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        void Set<T>(T value, CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task{TResult}"/> that returns <typeparamref name="T"/>.</returns>
        Task<T?> GetAsync<T>(CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The value of type <typeparamref name="T"/>.</returns>
        T? Get<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RefreshAsync<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        void Refresh<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RemoveAsync<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        void Remove<T>(CancellationToken token = default);
    }
    
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