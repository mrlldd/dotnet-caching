using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Stores.Internal
{
    internal class DistributedCacheStore : ICacheStore<InDistributed>
    {
        private readonly IDistributedCache distributedCache;

        public DistributedCacheStore(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public Result<T?> Get<T>(string key, ICacheStoreOperationOptions operationOptions) 
            => Result.Of(() =>
            {
                var fromCache = distributedCache.Get(key);

                if (fromCache == null || fromCache.Length == 0)
                {
                    throw new CacheMissException(key);
                }

                var deserialized = operationOptions.Serializer.DeserializeAsync<T>(fromCache).GetAwaiter().GetResult();
                if (deserialized.Successful)
                {
                    return deserialized.UnwrapAsSuccess();
                }

                throw new DeserializationFailException(key, fromCache, typeof(T), deserialized);
            });

        public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var task = Result.Of(async () =>
            {
                var fromCache = await distributedCache.GetAsync(key, token);
                if (fromCache == null || fromCache.Length == 0)
                {
                    throw new CacheMissException(key);
                }

                var deserialized = await operationOptions.Serializer.DeserializeAsync<T>(fromCache, token);
                if (deserialized.Successful)
                {
                    return deserialized.UnwrapAsSuccess();
                }

                throw new DeserializationFailException(key, fromCache, typeof(T), deserialized);
            });
            return new ValueTask<Result<T?>>(task);
        }

        public Result Set<T>(string key, T? value, CachingOptions options, ICacheStoreOperationOptions operationOptions) 
            => Result.Of(() =>
            {
                var serialized = operationOptions.Serializer.SerializeAsync(value).GetAwaiter().GetResult();
                if (!serialized.Successful)
                {
                    throw new SerializationFailException(key, value, typeof(T), serialized);
                }
                
                distributedCache.Set(key, serialized, new DistributedCacheEntryOptions
                {
                    SlidingExpiration = options.SlidingExpiration,
                    AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
                });
            });

        public ValueTask<Result> SetAsync<T>(string key, T? value, CachingOptions options,
            ICacheStoreOperationOptions operationOptions, CancellationToken token = default)
        {
            var task = Result.Of(async () =>
            {
                var serialized = await operationOptions.Serializer.SerializeAsync(value, token);
                if (!serialized.Successful)
                {
                    throw new SerializationFailException(key, value, typeof(T), serialized);
                }
                
                await distributedCache.SetAsync(key, serialized,
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = options.SlidingExpiration,
                        AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
                    }, token);
            });
            return new ValueTask<Result>(task);
        }

        public Result Refresh(string key, ICacheStoreOperationOptions operationOptions) 
            => Result.Of(() => distributedCache.Refresh(key));

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var task = Result.Of(() => distributedCache.RefreshAsync(key, token));
            return new ValueTask<Result>(task);
        }

        public Result Remove(string key, ICacheStoreOperationOptions operationOptions) 
            => Result.Of(() => distributedCache.Remove(key));

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            var task = Result.Of(() => distributedCache.RemoveAsync(key, token));
            return new ValueTask<Result>(task);
        }
    }
}