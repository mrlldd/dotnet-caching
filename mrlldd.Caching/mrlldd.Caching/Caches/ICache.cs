using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Caches
{
    public interface ICache<T> : ICaching<T>
    {
        Task SetAsync(T value, CancellationToken token = default);
        Task<T> GetAsync(CancellationToken token = default);
    }
}