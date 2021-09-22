using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Strategies;

namespace mrlldd.Caching.Caches
{
    public interface ICache<T>
    {
        IReadOnlyCachesCollection<T> Instances { get; }
        
        Task<Result<T?>> GetAsync(CancellationToken token = default);

        Result<T?> Get();

        Task<Result<T?>> GetAsync(ICachingGetStrategy strategy, CancellationToken token = default);

        Result<T?> Get(ICachingGetStrategy strategy);

        Task<Result> SetAsync(T value, CancellationToken token = default);

        Result Set(T value);

        Task<Result> SetAsync(T value, ICachingSetStrategy strategy, CancellationToken token = default);

        Result Set(T value, ICachingSetStrategy strategy);

        Task<Result> RefreshAsync(CancellationToken token = default);

        Result Refresh();

        Task<Result> RefreshAsync(ICachingRefreshStrategy strategy, CancellationToken token = default);

        Result Refresh(ICachingRefreshStrategy strategy);

        Task<Result> RemoveAsync(CancellationToken token = default);

        Result Remove();

        Task<Result> RemoveAsync(ICachingRemoveStrategy strategy, CancellationToken token = default);

        Result Remove(ICachingRemoveStrategy strategy);
    }


    /// <summary>
    /// The base interface for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    /// <typeparam name="TFlag"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface ICache<T, TFlag> : IUnknownStoreCache<T>
        where TFlag : CachingFlag
    {

    }
} 