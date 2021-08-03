using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace mrlldd.Caching.Tests.Caches.TestUtilities
{
    internal class FakeDistributedCache : IDistributedCache
    {
        private readonly IMemoryCache memoryCache;
        
        public FakeDistributedCache(IOptions<MemoryCacheOptions> options, ILoggerFactory loggerFactory) 
            // as IMemoryCache injected service sometimes can be a mocked IMemoryCache
            => memoryCache = new MemoryCache(options, loggerFactory);

        public byte[] Get(string key) 
            => memoryCache.Get<byte[]>(key);

        public Task<byte[]> GetAsync(string key, CancellationToken token = default) 
            => Task.FromResult(memoryCache.Get<byte[]>(key));

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) 
            => memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                SlidingExpiration = options.SlidingExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow
            });

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            Set(key, value, options);
            return Task.CompletedTask;
        }

        public void Refresh(string key) 
            => memoryCache.TryGetValue(key, out _);

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            memoryCache.TryGetValue(key, out _);
            return Task.CompletedTask;
        }

        public void Remove(string key) 
            => memoryCache.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            memoryCache.Remove(key);
            return Task.CompletedTask;
        }
    }
}