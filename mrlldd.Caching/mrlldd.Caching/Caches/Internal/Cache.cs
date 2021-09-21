﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;

namespace mrlldd.Caching.Caches.Internal
{
    internal class Cache<T> : ICache<T>
    {
        public IReadOnlyCachesCollection<T> Caches { get; }

        public Cache(IReadOnlyCachesCollection<T> caches)
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
}