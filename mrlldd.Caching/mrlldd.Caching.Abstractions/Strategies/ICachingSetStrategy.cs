using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    public interface ICachingSetStrategy
    {
        Task<Result> SetAsync<T>(IReadOnlyCachesCollection<T> caches, T value, CancellationToken token = default);

        Result Set<T>(IReadOnlyCachesCollection<T> caches, T value);
    }
}