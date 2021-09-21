using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Strategies;

namespace mrlldd.Caching.Caches.Internal
{
    internal class Cache<T> : ICache<T>
    {
        public IReadOnlyCachesCollection<T> Instances { get; }

        public Cache(IReadOnlyCachesCollection<T> instances)
            => Instances = instances;

        public Task<Result<T?>> GetAsync(CancellationToken token = default)
            => GetAsync(GetFirstSuccessfulStrategy.Instance, token);

        public Result<T?> Get() => Get(GetFirstSuccessfulStrategy.Instance);

        public Task<Result<T?>> GetAsync(ICachingGetStrategy strategy, CancellationToken token = default)
            => strategy.GetAsync(Instances, token);

        public Result<T?> Get(ICachingGetStrategy strategy)
            => strategy.Get(Instances);

        public Task<Result> SetAsync(T value, CancellationToken token = default)
            => SetAsync(value, ParallelStrategy.Instance, token);

        public Result Set(T value)
            => Set(value, SequenceStrategy.Instance);

        public Task<Result> SetAsync(T value, ICachingSetStrategy strategy, CancellationToken token = default) 
            => strategy.SetAsync(Instances, value, token);

        public Result Set(T value, ICachingSetStrategy strategy)
            => strategy.Set(Instances, value);

        public Task<Result> RefreshAsync(CancellationToken token = default)
            => RefreshAsync(ParallelStrategy.Instance, token);

        public Result Refresh()
            => Refresh(SequenceStrategy.Instance);

        public Task<Result> RefreshAsync(ICachingRefreshStrategy strategy, CancellationToken token = default)
            => strategy.RefreshAsync(Instances, token);

        public Result Refresh(ICachingRefreshStrategy strategy)
            => strategy.Refresh(Instances);

        public Task<Result> RemoveAsync(CancellationToken token = default)
            => RemoveAsync(ParallelStrategy.Instance, token);

        public Result Remove()
            => Remove(SequenceStrategy.Instance);

        public Task<Result> RemoveAsync(ICachingRemoveStrategy strategy, CancellationToken token = default)
            => strategy.RemoveAsync(Instances, token);

        public Result Remove(ICachingRemoveStrategy strategy)
            => strategy.Remove(Instances);
    }
}