using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Microsoft.Extensions.Caching.Memory;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class MemoryCacheStore : ICacheStore<InMemory>
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheStore(IMemoryCache memoryCache)
            => this.memoryCache = memoryCache;

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() =>
            {
                var fromCache = memoryCache.Get<T>(key);
                return fromCache != null ? fromCache : default;
            });

        public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => new(Get<T>(key, metadata));

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
            => Result.Of(new Action(() => memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                SlidingExpiration = options.SlidingExpiration
            })));

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                memoryCache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = options.SlidingExpiration
                });
            });
            return new ValueTask<Result>(result);
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(new Action(() => memoryCache.Get<byte[]>(key)));

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                memoryCache.Get(key);
            }); 
            return new ValueTask<Result>(result);
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => memoryCache.Remove(key));

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var result = Result.Of(() =>
            {
                memoryCache.Remove(key);
            }); 
            return new ValueTask<Result>(result);
        }
    }
}