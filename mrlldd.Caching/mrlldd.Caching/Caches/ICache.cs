using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public interface ICache<T> : ICaching<T>
    {
        /// <summary>
        /// The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        Task SetAsync(T value, CancellationToken token = default);
        /// <summary>
        /// The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data.</returns>
        Task<T> GetAsync(CancellationToken token = default);
    }
}