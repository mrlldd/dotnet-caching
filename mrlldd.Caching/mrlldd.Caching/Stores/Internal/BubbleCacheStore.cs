using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace mrlldd.Caching.Stores.Internal
{
    internal class BubbleCacheStore : IBubbleCacheStore
    {
        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => default(T).AsSuccess();

        public Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult(default(T).AsSuccess());

        public Result Set<T>(string key, T value, MemoryCacheEntryOptions options, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> SetAsync<T>(string key, T value, MemoryCacheEntryOptions options,
            ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Task.FromResult(Result.Success);

        public Result Set<T>(string key, T value, DistributedCacheEntryOptions options,
            ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> SetAsync<T>(string key, T value, DistributedCacheEntryOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult(Result.Success);

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> RefreshAsync(string key,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult(Result.Success);

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Task.FromResult(Result.Success);
    }
}