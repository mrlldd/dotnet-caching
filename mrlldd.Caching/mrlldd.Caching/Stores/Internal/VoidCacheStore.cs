using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class VoidCacheStore : ICacheStore<InVoid>
    {
        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => new CacheMissException(key);

        public Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult<Result<T?>>(new CacheMissException(key));

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult(Result.Success);

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Task.FromResult(Result.Success);

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Success;

        public Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Task.FromResult(Result.Success);
    }
}