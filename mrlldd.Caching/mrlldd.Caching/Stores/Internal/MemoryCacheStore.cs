using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Microsoft.Extensions.Caching.Memory;

namespace mrlldd.Caching.Stores.Internal
{
    internal class MemoryCacheStore : CachingStore, IMemoryCacheStore
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheStore(IMemoryCache memoryCache)
            => this.memoryCache = memoryCache;

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() =>
            {
                var fromCache = memoryCache.Get<byte[]>(key);
                return fromCache != null && fromCache.Any() ? Deserialize<T>(fromCache) : default;
            });

        public Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Get<T>(key, metadata).Map(Task.FromResult);

        public Result Set<T>(string key, T value, MemoryCacheEntryOptions options, ICacheStoreOperationMetadata metadata)
            => Result.Of(new Action(() => memoryCache.Set(key, Serialize(value), options)));

        public Task<Result> SetAsync<T>(string key, T value, MemoryCacheEntryOptions options, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => Result.Of(() =>
            {
                memoryCache.Set(key, Serialize(value), options);
                return Task.CompletedTask;
            });

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(new Action(() => memoryCache.Get<byte[]>(key)));

        public Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => Result.Of(() =>
            {
                memoryCache.Get<byte[]>(key);
                return Task.CompletedTask;
            });

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => Result.Of(() => memoryCache.Remove(key));

        public Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default) 
            => Result.Of(() =>
            {
                memoryCache.Remove(key);
                return Task.CompletedTask;
            });
    }
}