using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Functional.Result.Extensions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Loaders
{
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
        public async ValueTask<Result<TResult>> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var gettingTask = TryGetFromCacheAsync(keySuffix, token);
                var inCache = gettingTask.IsCompletedSuccessfully 
                    ? gettingTask.Result
                    : await gettingTask;
                if (inCache.Successful)
                {
                    return inCache.UnwrapAsSuccess();
                }
            }

            var loaded = await LoadAsync(args, token);
            var cachingTask = PerformCachingAsync(loaded, keySuffix, token);
            if (!cachingTask.IsCompletedSuccessfully)
            {
                await cachingTask;
            }

            return loaded;
        }

        /// <inheritdoc />
        public Result<TResult> GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var fromCache = TryGetFromCache(keySuffix);
                if (fromCache.Successful)
                {
                    return fromCache;
                }
            }

            var loaded = LoadAsync(args, token).GetAwaiter().GetResult();
            PerformCaching(loaded, keySuffix);
            return loaded;
        }

        /// <inheritdoc />
        public ValueTask<Result> SetAsync(TArgs args, TResult result, CancellationToken token = default)
            => PerformCachingAsync(result, CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result Set(TArgs args, TResult result)
            => PerformCaching(result, CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public ValueTask<Result<TResult>> GetAsync(TArgs args, CancellationToken token = default)
            => TryGetFromCacheAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result<TResult> Get(TArgs args)
            => TryGetFromCache(CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public ValueTask<Result> RefreshAsync(TArgs args, CancellationToken token = default)
            => RefreshAsync(CacheKeySuffixFactory(args), token);

        /// <inheritdoc />
        public Result Refresh(TArgs args)
            => Refresh(CacheKeySuffixFactory(args));

        /// <inheritdoc />
        public ValueTask<Result> RemoveAsync(TArgs args, CancellationToken token = default)
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
        protected abstract Task<TResult> LoadAsync(TArgs args, CancellationToken token = default);

        /// <summary>
        /// The abstract method for creating cache key suffix in order to make stored items keys really unique,
        /// the returned string should be kinda hash of argument.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The additional cache key suffix.</returns>
        protected abstract string CacheKeySuffixFactory(TArgs args);
    }
}