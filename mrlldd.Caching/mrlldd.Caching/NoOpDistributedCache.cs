using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace mrlldd.Caching
{
    internal class NoOpDistributedCache : IDistributedCache
    {
        public byte[] Get(string key) 
            => null;

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
            => Task.FromResult<byte[]>(null);

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options,
            CancellationToken token = default) 
            => Task.CompletedTask;

        public void Refresh(string key)
        {
        }

        public Task RefreshAsync(string key, CancellationToken token = default) 
            => Task.CompletedTask;

        public void Remove(string key)
        {
        }

        public Task RemoveAsync(string key, CancellationToken token = default) 
            => Task.CompletedTask;
    }
}