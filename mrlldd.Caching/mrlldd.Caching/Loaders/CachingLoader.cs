using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using mrlldd.Caching.Internal;

namespace mrlldd.Caching.Loaders
{
    internal sealed class CachingLoader : ICachingLoader
    {
        private readonly ILoaderProvider loaderProvider;

        public CachingLoader(ILoaderProvider loaderProvider)
            => this.loaderProvider = loaderProvider;

        public Task<TResult?> GetOrLoadAsync<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) where TResult : class => GetLoader<TArgs, TResult>()
            .GetOrLoadAsync(args, omitCacheOnLoad, token);

        public TResult? GetOrLoad<TArgs, TResult>(TArgs args, bool omitCacheOnLoad = false,
            CancellationToken token = default) where TResult : class => GetLoader<TArgs, TResult>()
            .GetOrLoad(args, omitCacheOnLoad, token);

        public Task SetAsync<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().SetAsync(args, result, token);

        public void Set<TArgs, TResult>(TArgs args, TResult result, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Set(args, result, token);

        public Task<TResult?> GetAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().GetAsync(args, token);

        public TResult? Get<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Get(args, token);

        public Task RefreshAsync<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().RefreshAsync(args, token);

        public void Refresh<TArgs, TResult>(TArgs args, CancellationToken token = default)
            where TResult : class
            => GetLoader<TArgs, TResult>().Refresh(args, token);

        public Task RemoveAsync<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class => GetLoader<TArgs, TResult>().RemoveAsync(args, token);

        public void Remove<TArgs, TResult>(TArgs args, CancellationToken token = default) where TResult : class => GetLoader<TArgs, TResult>().Remove(args, token);

        private ICachingLoader<TArgs, TResult> GetLoader<TArgs, TResult>() where TResult : class => loaderProvider.GetRequired<TArgs, TResult>()
                .Map(x => x.Successful
                    ? x.UnwrapAsSuccess()
                    : throw x);
    }

    /// <summary>
    /// The base class for implemented caching loaders
    /// </summary>
    /// <typeparam name="TArgs">Loading argument type.</typeparam>
    /// <typeparam name="TResult">Loading result type.</typeparam>
    public abstract class CachingLoader<TArgs, TResult> : Caching<TResult>, ICachingLoader<TArgs, TResult>, IInternalLoaderService<TArgs, TResult>
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