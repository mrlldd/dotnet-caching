using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    public interface ICachingGetStrategy
    {
        Task<Result<T?>> GetAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);
        
        Result<T?> Get<T>(IReadOnlyCachesCollection<T> caches);
    }
}