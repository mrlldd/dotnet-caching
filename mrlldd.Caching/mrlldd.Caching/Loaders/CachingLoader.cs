using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders.Internal;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    ///     The base class for implemented caching loaders.
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    /// <typeparam name="TStoreFlag">The cache store flag type.</typeparam>
    public abstract class CachingLoader<TArgs, TResult, TStoreFlag> : Caching<TResult, TStoreFlag>,
        ICachingLoader<TArgs, TResult, TStoreFlag>, IInternalLoader<TArgs, TResult, TStoreFlag>
        where TResult : class
        where TStoreFlag : CachingFlag
    {
        /// <summary>
        ///     The loader service.
        /// </summary>
        protected ILoader<TArgs, TResult> Loader { get; private set; } = null!;

        /// <summary>
        ///     The sealed factory method used for creating the global cache key prefixes.
        /// </summary>
        /// <returns>The collection of prefixes.</returns>
        protected sealed override string CacheKeyPrefix
            => "loader";

        /// <inheritdoc />
        public async ValueTask<Result<TResult?>> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var gettingTask = TryGetFromCacheAsync(keySuffix, token);
                var fromCache = gettingTask.IsCompletedSuccessfully
                    ? gettingTask.Result
                    : await gettingTask;
                if (fromCache.Successful) return fromCache;
            }

            var loaded = await Loader.LoadAsync(args, token);
            var onLoadFinishTask = OnLoadFinishAsync(args, loaded, token);
            if (!onLoadFinishTask.IsCompletedSuccessfully)
            {
                await onLoadFinishTask;
            }
            var cachingTask = PerformCachingAsync(loaded, keySuffix, token);
            if (!cachingTask.IsCompletedSuccessfully)
            {
                await cachingTask;
            }

            return loaded;
        }

        /// <inheritdoc />
        public Result<TResult?> GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var fromCache = TryGetFromCache(keySuffix);
                if (fromCache.Successful) return fromCache;
            }

            var loaded = Loader.LoadAsync(args, token).GetAwaiter().GetResult();
            var onLoadFinishTask = OnLoadFinishAsync(args, loaded, token);
            if (!onLoadFinishTask.IsCompletedSuccessfully)
            {
                onLoadFinishTask.GetAwaiter().GetResult();
            }
            PerformCaching(loaded, keySuffix);
            return loaded;
        }

        /// <inheritdoc />
        public ValueTask<Result> SetAsync(TArgs args, TResult? result, CancellationToken token = default)
        {
            return PerformCachingAsync(result, CacheKeySuffixFactory(args), token);
        }

        /// <inheritdoc />
        public Result Set(TArgs args, TResult? result)
        {
            return PerformCaching(result, CacheKeySuffixFactory(args));
        }

        /// <inheritdoc />
        public ValueTask<Result<TResult?>> GetAsync(TArgs args, CancellationToken token = default)
        {
            return TryGetFromCacheAsync(CacheKeySuffixFactory(args), token);
        }

        /// <inheritdoc />
        public Result<TResult?> Get(TArgs args)
        {
            return TryGetFromCache(CacheKeySuffixFactory(args));
        }

        /// <inheritdoc />
        public ValueTask<Result> RefreshAsync(TArgs args, CancellationToken token = default)
        {
            return RefreshAsync(CacheKeySuffixFactory(args), token);
        }

        /// <inheritdoc />
        public Result Refresh(TArgs args)
        {
            return Refresh(CacheKeySuffixFactory(args));
        }

        /// <inheritdoc />
        public ValueTask<Result> RemoveAsync(TArgs args, CancellationToken token = default)
        {
            return RemoveAsync(CacheKeySuffixFactory(args), token);
        }

        /// <inheritdoc />
        public Result Remove(TArgs args)
        {
            return Remove(CacheKeySuffixFactory(args));
        }

        /// <inheritdoc />
        protected sealed override void EnrichWithDependencies(IServiceProvider serviceProvider)
        {
            base.EnrichWithDependencies(serviceProvider);
            var foundLoader = serviceProvider.GetService<ILoader<TArgs, TResult>>();
            Loader = foundLoader ?? throw new LoaderNotFoundException<TArgs, TResult>();
        }

        /// <summary>
        ///     The abstract method for creating cache key suffix in order to make stored items keys really unique,
        ///     the returned string should be kinda hash of argument.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The additional cache key suffix.</returns>
        protected abstract string CacheKeySuffixFactory(TArgs args);

        /// <summary>
        ///     The virtual method for performing side effects after load finish. 
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">The result.</param>
        /// <param name="token">The cancellation token.</param>
        /// <returns>The value task.</returns>
        protected virtual ValueTask OnLoadFinishAsync(TArgs args, TResult result, CancellationToken token) => new();
    }
}