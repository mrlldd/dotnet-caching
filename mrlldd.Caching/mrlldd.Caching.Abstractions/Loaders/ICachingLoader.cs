using System.Threading;
using System.Threading.Tasks;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    /// The interface that represents caching loader service-wrapper that provides generic access to generic loaders.
    /// </summary>
    public interface ICachingLoader
    {
        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <typeparam name="TArgs">The type of arguments.</typeparam>
        /// <typeparam name="TResult">The type of result.</typeparam>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        public Task<TResult?> GetOrLoadAsync<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        public TResult? GetOrLoad<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task SetAsync<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class;

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        public void Set<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class;

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        public Task<TResult?> GetAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class;

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        public TResult? Get<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task RefreshAsync<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        public void Refresh<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task RemoveAsync<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class;

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        public void Remove<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class;
    }

    /// <summary>
    /// The base class for implemented caching loaders
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    public interface ICachingLoader<in TArgs, TResult> : ICaching
    {
        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        Task<TResult?> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        TResult? GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task SetAsync(TArgs args, TResult result, CancellationToken token = default);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        void Set(TArgs args, TResult result, CancellationToken token = default);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        Task<TResult?> GetAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        TResult? Get(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task RefreshAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        void Refresh(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        Task RemoveAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        void Remove(TArgs args, CancellationToken token = default);
    }
}