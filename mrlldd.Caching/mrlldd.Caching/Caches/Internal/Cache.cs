using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Strategies;

namespace mrlldd.Caching.Caches.Internal
{
    internal class Cache<T> : ICache<T>
    {
        public Cache(IReadOnlyCachesCollection<T> instances)
        {
            Instances = instances;
        }

        public IReadOnlyCachesCollection<T> Instances { get; }

        public Task<Result<T>> GetAsync(CancellationToken token = default)
        {
            return GetAsync(GetFirstSuccessfulStrategy.Instance, token);
        }

        public Result<T> Get()
        {
            return Get(GetFirstSuccessfulStrategy.Instance);
        }

        public Task<Result<T>> GetAsync(ICacheGetStrategy strategy, CancellationToken token = default)
        {
            return strategy.GetAsync(Instances, token);
        }

        public Result<T> Get(ICacheGetStrategy strategy)
        {
            return strategy.Get(Instances);
        }

        public Task<Result> SetAsync(T value, CancellationToken token = default)
        {
            return SetAsync(value, ParallelStrategy.Instance, token);
        }

        public Result Set(T value)
        {
            return Set(value, SequenceStrategy.Instance);
        }

        public Task<Result> SetAsync(T value, ICachingSetStrategy strategy, CancellationToken token = default)
        {
            return strategy.SetAsync(Instances, value, token);
        }

        public Result Set(T value, ICachingSetStrategy strategy)
        {
            return strategy.Set(Instances, value);
        }

        public Task<Result> RefreshAsync(CancellationToken token = default)
        {
            return RefreshAsync(ParallelStrategy.Instance, token);
        }

        public Result Refresh()
        {
            return Refresh(SequenceStrategy.Instance);
        }

        public Task<Result> RefreshAsync(ICachingRefreshStrategy strategy, CancellationToken token = default)
        {
            return strategy.RefreshAsync(Instances, token);
        }

        public Result Refresh(ICachingRefreshStrategy strategy)
        {
            return strategy.Refresh(Instances);
        }

        public Task<Result> RemoveAsync(CancellationToken token = default)
        {
            return RemoveAsync(ParallelStrategy.Instance, token);
        }

        public Result Remove()
        {
            return Remove(SequenceStrategy.Instance);
        }

        public Task<Result> RemoveAsync(ICachingRemoveStrategy strategy, CancellationToken token = default)
        {
            return strategy.RemoveAsync(Instances, token);
        }

        public Result Remove(ICachingRemoveStrategy strategy)
        {
            return strategy.Remove(Instances);
        }
    }
}