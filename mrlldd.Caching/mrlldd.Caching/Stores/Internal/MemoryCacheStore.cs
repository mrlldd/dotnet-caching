using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class MemoryCacheStore : ICacheStore<InMemory>
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheStore(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public Result<T> Get<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            return Result.Of(() => memoryCache.Get<T>(key))
                .Bind(t => t != null ? t : throw new CacheMissException(key));
        }

        public ValueTask<Result<T>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            return new(Get<T>(key, operationOptions));
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationOptions operationOptions)
        {
            return Result.Of(new Action(() => memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = options.SlidingExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
            })));
        }

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = options.SlidingExpiration,
                    AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
                });
            });
            return new ValueTask<Result>(result);
        }

        public Result Refresh(string key, ICacheStoreOperationOptions operationOptions)
        {
            return Result.Of(new Action(() => memoryCache.Get(key)));
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var result = Result.Of(new Action(() => memoryCache.Get(key)));
            return new ValueTask<Result>(result);
        }

        public Result Remove(string key, ICacheStoreOperationOptions operationOptions)
        {
            return Result.Of(() => memoryCache.Remove(key));
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var result = Result.Of(() => { memoryCache.Remove(key); });
            return new ValueTask<Result>(result);
        }
    }
}