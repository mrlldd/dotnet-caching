using System.Threading;
using System.Threading.Tasks;
using Functional.Result;

namespace mrlldd.Caching.Stores
{
    public interface ICachingStore<in TOptions>
    {
        Result<T> Get<T>(string key);

        Task<Result<T>> GetAsync<T>(string key, CancellationToken token = default);

        Result Set<T>(string key, T value, TOptions options);

        Task<Result> SetAsync<T>(string key, T value, TOptions options, CancellationToken token = default);

        Result Refresh(string key);

        Task<Result> RefreshAsync(string key, CancellationToken token = default);

        Result Remove(string key);

        Task<Result> RemoveAsync(string key, CancellationToken token = default);
    }
}