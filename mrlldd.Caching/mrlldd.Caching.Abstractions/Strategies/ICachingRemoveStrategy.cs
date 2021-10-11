using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    public interface ICachingRemoveStrategy
    {
        Task<Result> RemoveAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);

        Result Remove<T>(IReadOnlyCachesCollection<T> caches);
    }
}