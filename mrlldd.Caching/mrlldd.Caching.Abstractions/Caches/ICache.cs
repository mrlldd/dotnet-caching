using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;

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
        Task<Result> SetAsync<T>(T value, CancellationToken token = default);
        
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        Result Set<T>(T value);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task{TResult}"/> that returns <typeparamref name="T"/>.</returns>
        Task<Result<T?>> GetAsync<T>(CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The value of type <typeparamref name="T"/>.</returns>
        Result<T?> Get<T>();

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<Result> RefreshAsync<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        Result Refresh<T>();

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<Result> RemoveAsync<T>(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="T">The type of value.</typeparam>
        Result Remove<T>();
    }
    
    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public interface ICache<T> : ICaching
    {
        //todo fix xml comments
        
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        Task<Result> SetAsync(T value, CancellationToken token = default);

        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        public Result Set(T value);

        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Task<Result<T?>> GetAsync(CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        public Result<T?> Get();

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RefreshAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        Result Refresh();

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RemoveAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        Result Remove();
    }
    
    // ReSharper disable once UnusedTypeParameter
    public interface ICache<T, TFlag> : ICache<T> where TFlag : CachingFlag
    {
        
    }
} 