using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Caches
{
    public interface IUnknownStoreCache<T>
    {
        //todo fix xml comments
        
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        ValueTask<Result> SetAsync(T value, CancellationToken token = default);

        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        Result Set(T value);

        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        ValueTask<Result<T>> GetAsync(CancellationToken token = default);
        
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Result<T> Get();

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        ValueTask<Result> RefreshAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing data expiration in cache.
        /// </summary>
        Result Refresh();

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        ValueTask<Result> RemoveAsync(CancellationToken token = default);

        /// <summary>
        /// The method used for removing data from cache.
        /// </summary>
        Result Remove();
    }
}