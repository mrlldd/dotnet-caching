using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Extensions;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Caches
{
    internal class Cache<T> : ICache<T>
    {
        public IReadOnlyCollection<IUnknownStoreCache<T>> Caches { get; }

        public Cache(IReadOnlyCollection<IUnknownStoreCache<T>> caches)
            => Caches = caches;

    }
    
     /// <inheritdoc />
    internal sealed class Cache : ICache
    {
        private readonly ICacheProvider cacheProvider;

        public Cache(ICacheProvider cacheProvider) 
            => this.cacheProvider = cacheProvider;

        public Task<Result> SetAsync<T>(T value, CancellationToken token = default)
            => GetCache<T>().SetAsync(value, token);

        public Result Set<T>(T value)
            => GetCache<T>().Set(value);

        public Task<Result<T?>> GetAsync<T>(CancellationToken token = default)
            => GetCache<T>().GetAsync(token);

        public Result<T?> Get<T>()
            => GetCache<T>().Get();

        public Task<Result> RefreshAsync<T>(CancellationToken token = default)
            => GetCache<T>().RefreshAsync(token);

        public Result Refresh<T>()
            => GetCache<T>().Refresh();

        public Task<Result> RemoveAsync<T>(CancellationToken token = default)
            => GetCache<T>().RemoveAsync(token);

        public Result Remove<T>()
            => GetCache<T>().Remove();

        private IInternalCache<T> GetCache<T>()
            => cacheProvider.GetRequired<T>()
                .Map(x => x.Successful 
                    ? x.UnwrapAsSuccess() 
                    : throw x);
    }

    /// <summary>
    /// The base class for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TStoreFlag">The cache store flag type.</typeparam>
    public abstract class Cache<T, TStoreFlag> : Caching<T, TStoreFlag>,
     ICache<T, TStoreFlag>,
     IInternalCache<T, TStoreFlag> 
        where TStoreFlag : CachingFlag
    {
        /// <inheritdoc />
        protected sealed override string CacheKeyPrefix => "cache";

        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// The default key suffix for given cache type.
        /// </summary>
        protected virtual string DefaultKeySuffix { get; } = typeof(T).Map(x => $"{x.Namespace}.{x.Name}");

        /// <inheritdoc />
        public Task<Result> SetAsync(T value, CancellationToken token = default)
            => PerformCachingAsync(value, DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Set(T value)
            => PerformCaching(value, DefaultKeySuffix);

        /// <inheritdoc />
        public Task<Result<T?>> GetAsync(CancellationToken token = default)
            => TryGetFromCacheAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result<T?> Get()
            => TryGetFromCache(DefaultKeySuffix);

        /// <inheritdoc />
        public Task<Result> RefreshAsync(CancellationToken token = default)
            => RefreshAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Refresh()
            => Refresh(DefaultKeySuffix);

        /// <inheritdoc />
        public Task<Result> RemoveAsync(CancellationToken token = default)
            => RemoveAsync(DefaultKeySuffix, token);

        /// <inheritdoc />
        public Result Remove()
            => Remove(DefaultKeySuffix);
    }
}