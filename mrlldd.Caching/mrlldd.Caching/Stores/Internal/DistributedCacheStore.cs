using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Microsoft.Extensions.Caching.Distributed;

namespace mrlldd.Caching.Stores.Internal
{
    internal class DistributedCacheStore : CachingStore, IDistributedCacheStore
    {
        private readonly IDistributedCache distributedCache;

        public DistributedCacheStore(IDistributedCache distributedCache)
            => this.distributedCache = distributedCache;

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Get(key).Map(Deserialize<T>));

        public Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Result.Of(async () =>
            {
                var fromCache = await distributedCache.GetAsync(key, token);
                return fromCache != null && fromCache.Any()
                    ? Deserialize<T>(fromCache)
                    : default;
            });

        public Result Set<T>(string key, T value,
            DistributedCacheEntryOptions options, 
            ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Set(key, Serialize(value), options));

        public Task<Result> SetAsync<T>(string key, T value, DistributedCacheEntryOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Result.Of(() => distributedCache.SetAsync(key, Serialize(value), options, token));

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Refresh(key));

        public Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Result.Of(() => distributedCache.RefreshAsync(key, token));

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => distributedCache.Remove(key));

        public Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Result.Of(() => distributedCache.RemoveAsync(key, token));
    }
}