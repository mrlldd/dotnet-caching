using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class VoidCacheStore : ICacheStore<InVoid>
    {
        public Result<T> Get<T>(string key, ICacheStoreOperationMetadata metadata)
        {
            return new CacheMissException(key);
        }

        public ValueTask<Result<T>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return new(new CacheMissException(key));
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
        {
            return Result.Success;
        }

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return new(Result.Success);
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
        {
            return Result.Success;
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return new(Result.Success);
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
        {
            return Result.Success;
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return new(Result.Success);
        }
    }
}