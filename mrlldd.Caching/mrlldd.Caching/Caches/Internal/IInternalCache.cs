using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches.Internal
{
    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    internal interface IInternalCache<T> : ICaching
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
    
    internal interface IInternalCache<T, TFlag> : IInternalCache<T> where TFlag : CachingFlag
    {
    }
}