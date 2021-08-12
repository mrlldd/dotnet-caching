
using Functional.Result;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The service used for providing caches.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// The method used to get a cache.
        /// </summary>
        /// <typeparam name="T">The type of stored object in cache.</typeparam>
        /// <returns>The result of getting the cache.</returns>
        Result<ICache<T>> Get<T>();
    }
}