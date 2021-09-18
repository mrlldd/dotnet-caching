using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Loaders
{
    internal sealed class CachingLoader : ICachingLoader
    {
        private readonly ILoaderProvider loaderProvider;

        public CachingLoader(ILoaderProvider loaderProvider)
            => this.loaderProvider = loaderProvider;

        public Task<Result<TResult?>> GetOrLoadAsync<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) 
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .GetOrLoadAsync(args, omitCacheOnLoad, token);

        public Result<TResult?> GetOrLoad<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) 
            where TResult : class 
            => GetLoader<TArgs, TResult>()
            .GetOrLoad(args, omitCacheOnLoad, token);

        public Task<Result> SetAsync<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .SetAsync(args, result, token);

        public Result Set<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .Set(args, result);

        public Task<Result<TResult?>> GetAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .GetAsync(args, token);

        public Result<TResult?> Get<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .Get(args);

        public Task<Result> RefreshAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .RefreshAsync(args, token);

        public Result Refresh<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .Refresh(args);

        public Task<Result> RemoveAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>()
                .RemoveAsync(args, token);

        public Result Remove<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Remove(args);

        private ICachingLoader<TArgs, TResult> GetLoader<TArgs, TResult>()
            where TResult : class => loaderProvider
            .GetRequired<TArgs, TResult>()
            .Map(x => x.Successful
                ? x.UnwrapAsSuccess()
                : throw x);
    }

    /// <summary>
    /// The base class for implemented caching loaders
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    /// <typeparam name="TStoreFlag">The cache store flag type.</typeparam>
    public abstract class CachingLoader<TArgs, TResult, TStoreFlag> : Caching<TResult, TStoreFlag>,
        ICachingLoader<TArgs, TResult>, IInternalLoaderService<TArgs, TResult>
        where TResult : class
        where TStoreFlag : CachingFlag
    {
        /// <inheritdoc />
        public async Task<Result<TResult?>> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = await TryGetFromCacheAsync(keySuffix, token);
                if (inCache.Successful)
                {
                    return inCache.UnwrapAsSuccess();
                }
            }

            var loaded = await LoadAsync(args, token);
            return await loaded.EffectAsync(x => PerformCachingAsync(x, keySuffix, token));
        }

        /// <inheritdoc />
        public Result<TResult?> GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = TryGetFromCache(keySuffix);
                if (inCache != null)
                {
                    return inCache;
                }
            }

            var loaded = LoadAsync(args, token).GetAwaiter().GetResult();
            PerformCaching(loaded, keySuffix);
            return loaded;
        }

        /// <inheritdoc />
        public Task<Result> SetAsync(TArgs args, TResult result, CancellationToken token = default)
            => PerformCachingAsync(result, CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result Set(TArgs args, TResult result)
            => PerformCaching(result, CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public Task<Result<TResult?>> GetAsync(TArgs args, CancellationToken token = default)
            => TryGetFromCacheAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result<TResult?> Get(TArgs args)
            => TryGetFromCache(CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public Task<Result> RefreshAsync(TArgs args, CancellationToken token = default)
            => RefreshAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result Refresh(TArgs args)
            => Refresh(CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public Task<Result> RemoveAsync(TArgs args, CancellationToken token = default)
            => RemoveAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result Remove(TArgs args)
            => Remove(CacheKeySuffixFactory(args));

        /// <summary>
        /// The sealed factory method used for creating the global cache key prefixes.
        /// </summary>
        /// <returns>The collection of prefixes.</returns>
        protected sealed override string CacheKeyPrefix
            => "loader";

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