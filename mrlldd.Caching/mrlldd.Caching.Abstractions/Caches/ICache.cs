using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Strategies;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    ///     The interface that represents caches with entries of type <typeparamref name="T" /> wrapper with unified
    ///     interaction methods.
    /// </summary>
    /// <typeparam name="T">The type of entries.</typeparam>
    public interface ICache<T>
    {
        /// <summary>
        ///     The caches of type <typeparamref name="T" /> instances.
        /// </summary>
        IReadOnlyCachesCollection<T> Instances { get; }

        /// <summary>
        ///     The method used for retrieving data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Task<Result<T?>> GetAsync(CancellationToken token = default);

        /// <summary>
        ///     The method used for retrieving data from cache.
        /// </summary>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Result<T?> Get();

        /// <summary>
        ///     The method used for retrieving data from cache.
        /// </summary>
        /// <param name="strategy">The caching get strategy.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Task<Result<T?>> GetAsync(ICacheGetStrategy strategy, CancellationToken token = default);

        /// <summary>
        ///     The method used for retrieving data from cache.
        /// </summary>
        /// <param name="strategy">The caching get strategy.</param>
        /// <returns>The cached data or null (if entry was not found or expired).</returns>
        Result<T?> Get(ICacheGetStrategy strategy);

        /// <summary>
        ///     The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="token">The cancellation token.</param>
        Task<Result> SetAsync(T value, CancellationToken token = default);

        /// <summary>
        ///     The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        Result Set(T value);

        /// <summary>
        ///     The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="strategy">The caching set strategy.</param>
        /// <param name="token">The cancellation token.</param>
        Task<Result> SetAsync(T value, ICachingSetStrategy strategy, CancellationToken token = default);

        /// <summary>
        ///     The method used for performing a caching.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="strategy">The caching set strategy.</param>
        Result Set(T value, ICachingSetStrategy strategy);

        /// <summary>
        ///     The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RefreshAsync(CancellationToken token = default);

        /// <summary>
        ///     The method used for refreshing data expiration in cache.
        /// </summary>
        Result Refresh();

        /// <summary>
        ///     The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="strategy">The caching refresh strategy.</param>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RefreshAsync(ICachingRefreshStrategy strategy, CancellationToken token = default);

        /// <summary>
        ///     The method used for refreshing data expiration in cache.
        /// </summary>
        /// <param name="strategy">The caching refresh strategy.</param>
        Result Refresh(ICachingRefreshStrategy strategy);

        /// <summary>
        ///     The method used for removing data from cache.
        /// </summary>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RemoveAsync(CancellationToken token = default);

        /// <summary>
        ///     The method used for removing data from cache.
        /// </summary>
        Result Remove();

        /// <summary>
        ///     The method used for removing data from cache.
        /// </summary>
        /// <param name="strategy">The caching remove strategy.</param>
        /// <param name="token">The cancellation token.</param>
        Task<Result> RemoveAsync(ICachingRemoveStrategy strategy, CancellationToken token = default);

        /// <summary>
        ///     The method used for removing data from cache.
        /// </summary>
        /// <param name="strategy">The caching remove strategy.</param>
        Result Remove(ICachingRemoveStrategy strategy);
    }


    /// <summary>
    ///     The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TFlag"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICache<T, TFlag> : IUnknownStoreCache<T>
        where TFlag : CachingFlag
    {
    }
}