using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Stores
{
    /// <summary>
    /// The interface that represents caching store and provides cache operations.
    /// </summary>
    /// <typeparam name="TOptions">The generic parameter that represents cache entry options.</typeparam>
    public interface ICacheStore<in TOptions>
    {
        /// <summary>
        /// The method for getting cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <typeparam name="T">The result type.</typeparam>
        /// <returns>The <see cref="Result{T}"/> with value of type <typeparamref name="T"/>.</returns>
        Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata);

        /// <summary>
        /// The method for getting cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <typeparam name="T">The result type.</typeparam>
        /// <returns>The <see cref="Task{T}"/> that returns <see cref="Result{T}"/> with value of type <typeparamref name="T"/>.</returns>
        Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default);

        /// <summary>
        /// The method for setting cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The cache entry options.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <typeparam name="T">The result type.</typeparam>
        /// <returns>The <see cref="Result"/>.</returns>
        Result Set<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata);

        /// <summary>
        /// The method for setting cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="value">The value.</param>
        /// <param name="options">The cache entry options.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <typeparam name="T">The result type.</typeparam>
        /// <returns>The <see cref="Task{T}"/> that returns <see cref="Result"/>.</returns>
        Task<Result> SetAsync<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default);

        /// <summary>
        /// The method for refreshing cache entry expiration.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <returns>The <see cref="Result"/>.</returns>
        Result Refresh(string key, ICacheStoreOperationMetadata metadata);

        /// <summary>
        /// The method for refreshing cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <returns>The <see cref="Task{T}"/> that returns <see cref="Result"/>.</returns>
        Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default);

        /// <summary>
        /// The method for removing cache entry expiration.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <returns>The <see cref="Result"/>.</returns>
        Result Remove(string key, ICacheStoreOperationMetadata metadata);

        /// <summary>
        /// The method for removing cache entry.
        /// </summary>
        /// <param name="key">The cache entry key.</param>
        /// <param name="metadata">The store operation metadata.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{T}"/> that returns <see cref="Result"/>.</returns>
        Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default);
    }
}