using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestImplementations.Flags;

namespace mrlldd.Caching.Tests.TestImplementations.Stores
{
    public class AbstractScopeStore : ICacheStore<InAbstractScope>
    {
        public Result<T?> Get<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Set<T>(string key, T? value, CachingOptions options, ICacheStoreOperationOptions operationOptions)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> SetAsync<T>(string key, T? value, CachingOptions options,
            ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Refresh(string key, ICacheStoreOperationOptions operationOptions)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Result Remove(string key, ICacheStoreOperationOptions operationOptions)
        {
            throw new NotImplementedException();
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}