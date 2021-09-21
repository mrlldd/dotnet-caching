using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    public interface IReadStrategy
    {
        Task<T?> ReadAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);
        
        T? Read<T>(IReadOnlyCachesCollection<T> caches);
    }
}