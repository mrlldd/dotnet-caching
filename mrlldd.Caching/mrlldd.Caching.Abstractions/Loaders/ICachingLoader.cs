using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    /// The base class for implemented caching loaders
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    /// <typeparam name="TFlag">The caching flag.</typeparam>
    public interface ICachingLoader<in TArgs, TResult, TFlag> : ICaching where TFlag : CachingFlag
    {
        // todo fix xml comments
        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        ValueTask<Result<TResult>> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        Result<TResult> GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        ValueTask<Result> SetAsync(TArgs args, TResult result, CancellationToken token = default);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        Result Set(TArgs args, TResult result);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        ValueTask<Result<TResult>> GetAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        Result<TResult> Get(TArgs args);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        ValueTask<Result> RefreshAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        Result Refresh(TArgs args);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        ValueTask<Result> RemoveAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        Result Remove(TArgs args);
    }
}