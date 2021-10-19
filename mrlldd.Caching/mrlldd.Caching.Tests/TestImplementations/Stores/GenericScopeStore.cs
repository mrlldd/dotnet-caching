using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestImplementations.Flags;

namespace mrlldd.Caching.Tests.TestImplementations.Stores
{
    public class GenericScopeStore : ICacheStore<InGenericScope>
    {
        public Result<T> Get<T>(string key, ICacheStoreOperationMetadata metadata)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<T>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}