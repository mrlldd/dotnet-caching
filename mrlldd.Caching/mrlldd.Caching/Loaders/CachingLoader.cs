using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result.Extensions;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    /// The cache service-wrapper that provides generic access to generic loaders.
    /// </summary>
    public sealed class CachingLoader
    {
        private readonly ILoaderProvider loaderProvider;

        /// <summary>
        /// The constructor for caching loader.
        /// </summary>
        /// <param name="loaderProvider">The loader provider.</param>
        public CachingLoader(ILoaderProvider loaderProvider)
            => this.loaderProvider = loaderProvider;

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
            CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().GetOrLoadAsync(args, omitCacheOnLoad, token);

        /// <summary>
        /// The method used for getting (if cached) or loading the object of result type.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="omitCacheOnLoad">The boolean that indicates if cache should be omitted on getting (means there should be only load and caching).</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        public TResult? GetOrLoad<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().GetOrLoad(args, omitCacheOnLoad, token);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task SetAsync<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().SetAsync(args, result, token);

        /// <summary>
        /// The method used for setting the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="result">The data.</param>
        /// <param name="token">The cancellation token.</param>
        public void Set<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Set(args, result, token);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns the object of result type.</returns>
        public Task<TResult?> GetAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().GetAsync(args, token);

        /// <summary>
        /// The method used for getting the object entry of result type from cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The object of result <typeparamref name="TResult"/> type.</returns>
        public TResult? Get<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Get(args, token);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task RefreshAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().RefreshAsync(args, token);

        /// <summary>
        /// The method used for refreshing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        public void Refresh<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Refresh(args, token);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The task.</returns>
        public Task RemoveAsync<TArgs, TResult>(TArgs args, CancellationToken token = default) 
            where TResult : class
            => GetLoader<TArgs, TResult>().RemoveAsync(args, token);

        /// <summary>
        /// The method used for removing the object entry of result type in cache.
        /// </summary>
        /// <param name="args">The argument.</param>
        /// <param name="token">The cancellation token.</param>
        public void Remove<TArgs, TResult>(TArgs args, CancellationToken token = default) 
            where TResult : class
            => GetLoader<TArgs, TResult>().Remove(args, token);

        private ICachingLoader<TArgs, TResult> GetLoader<TArgs, TResult>() where TResult : class
            => loaderProvider.GetRequired<TArgs, TResult>()
                .Map(x => x.Successful
                    ? x.UnwrapAsSuccess()
                    : throw x);
    }

    /// <summary>
    /// The base class for implemented caching loaders
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    public abstract class CachingLoader<TArgs, TResult> : Caching<TResult>, ICachingLoader<TArgs, TResult>
        where TResult : class
    {
        internal const string CacheKeyPrefix = "loader";

        /// <inheritdoc />
        public async Task<TResult?> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = await TryGetFromCacheAsync(keySuffix, token);
                if (inCache != null)
                {
                    return inCache;
                }
            }

            var loaded = await LoadAsync(args, token);
            return await loaded.EffectAsync(x => PerformCachingAsync(x, keySuffix, token));
        }

        /// <inheritdoc />
        public TResult? GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = TryGetFromCache(keySuffix, token);
                if (inCache != null)
                {
                    return inCache;
                }
            }

            var loaded = LoadAsync(args, token).GetAwaiter().GetResult();
            return loaded.Effect(x => PerformCaching(x, keySuffix, token));
        }

        /// <inheritdoc />
        public Task SetAsync(TArgs args, TResult result, CancellationToken token = default)
            => PerformCachingAsync(result, CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public void Set(TArgs args, TResult result, CancellationToken token = default)
            => PerformCaching(result, CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Task<TResult?> GetAsync(TArgs args, CancellationToken token = default)
            => TryGetFromCacheAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public TResult? Get(TArgs args, CancellationToken token = default)
            => TryGetFromCache(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Task RefreshAsync(TArgs args, CancellationToken token = default)
            => RefreshAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public void Refresh(TArgs args, CancellationToken token = default)
            => Refresh(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Task RemoveAsync(TArgs args, CancellationToken token = default)
            => RemoveAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public void Remove(TArgs args, CancellationToken token = default)
            => Remove(CacheKeySuffixFactory(args), token);

        /// <summary>
        /// The sealed factory method used for creating the global cache key prefixes.
        /// </summary>
        /// <returns>The collection of prefixes.</returns>
        protected sealed override IEnumerable<string> CacheKeyPrefixesFactory()
        {
            yield return CacheKeyPrefix;
        }

        /// <summary>
        /// The abstract method for loading of objects of result type.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The <see cref="Task{TResult}"/> that returns object of result type.</returns>
        protected abstract Task<TResult?> LoadAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The abstract method for creating cache key suffix in order to make stored items keys really unique,
        /// the returned string should be kinda hash of argument.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The additional cache key suffix.</returns>
        protected abstract string CacheKeySuffixFactory(TArgs args);
    }
}