using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;

namespace mrlldd.Caching.Strategies
{
    public interface ICachingRefreshStrategy
    {
        Task<Result> RefreshAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default);

        Result Refresh<T>(IReadOnlyCachesCollection<T> caches);
    }
}