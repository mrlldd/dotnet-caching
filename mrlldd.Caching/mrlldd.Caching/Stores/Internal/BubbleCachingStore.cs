using System;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace mrlldd.Caching.Stores.Internal
{
    internal class BubbleCachingStore : IBubbleCachingStore
    {
        public Result<T> Get<T>(string key)
            => default;

        public Task<Result<T>> GetAsync<T>(string key, CancellationToken token = default)
            => Task.FromResult(Result.Of(new Func<T>(() => default)));


        public Result Set<T>(string key, T value, MemoryCacheEntryOptions options) 
            => Result.Success;

        public Task<Result> SetAsync<T>(string key, T value, MemoryCacheEntryOptions options, CancellationToken token = default) 
            => Task.FromResult(Result.Success);

        public Result Set<T>(string key, T value, DistributedCacheEntryOptions options) 
            => Result.Success;

        public Task<Result> SetAsync<T>(string key, T value, DistributedCacheEntryOptions options,
            CancellationToken token = default) 
            => Task.FromResult(Result.Success);

        public Result Refresh(string key) 
            => Result.Success;

        public Task<Result> RefreshAsync(string key, CancellationToken token = default) 
            => Task.FromResult(Result.Success);

        public Result Remove(string key) 
            => Result.Success;

        public Task<Result> RemoveAsync(string key, CancellationToken token = default) 
            => Task.FromResult(Result.Success);
    }
}