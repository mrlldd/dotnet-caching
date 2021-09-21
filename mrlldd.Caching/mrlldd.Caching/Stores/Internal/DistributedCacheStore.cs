using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.Caching.Distributed;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class DistributedCacheStore : CachingStore, ICacheStore<InDistributed>
    {
        private readonly IDistributedCache distributedCache;

        public DistributedCacheStore(IDistributedCache distributedCache)
            => this.distributedCache = distributedCache;

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() =>
            {
                var fromCache = distributedCache.GetString(key);
                return string.IsNullOrEmpty(fromCache)
                    ? default
                    : Deserialize<T>(fromCache);
            });

        public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var task = Result.Of(async () =>
            {
                var fromCache = await distributedCache.GetStringAsync(key, token);
                return string.IsNullOrEmpty(fromCache)
                    ? default
                    : Deserialize<T>(fromCache);
            });
            return new ValueTask<Result<T?>>(task);
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.SetString(key, Serialize(value), new DistributedCacheEntryOptions
            {
                SlidingExpiration = options.SlidingExpiration
            }));

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata, CancellationToken token = default)
        {
            var task =  Result.Of(() => distributedCache.SetStringAsync(key, Serialize(value),
                new DistributedCacheEntryOptions
                {
                    SlidingExpiration = options.SlidingExpiration
                }, token));
            return new ValueTask<Result>(task);
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Refresh(key));

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var task = Result.Of(() => distributedCache.RefreshAsync(key, token));
            return new ValueTask<Result>(task);
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Remove(key));

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            var task = Result.Of(() => distributedCache.RemoveAsync(key, token));
            return new ValueTask<Result>(task);
        }
    }
}