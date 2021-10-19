using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Exceptions;

namespace mrlldd.Caching.Strategies
{
    /// <summary>
    ///     The class that represents a strategy that interacts with caches in parallel.
    /// </summary>
    public sealed class ParallelStrategy : ICachingRemoveStrategy, ICachingRefreshStrategy, ICachingSetStrategy
    {
        private ParallelStrategy()
        {
        }

        /// <summary>
        ///     The singleton instance.
        /// </summary>
        public static ParallelStrategy Instance { get; } = new();

        /// <inheritdoc />
        public Task<Result> RefreshAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
        {
            return ExecuteInParallelAsync(caches, (x, ct) => x.RefreshAsync(ct), token);
        }

        /// <inheritdoc />
        public Result Refresh<T>(IReadOnlyCachesCollection<T> caches)
        {
            return new NotSupportedSyncStrategyMethodException(nameof(ParallelStrategy), nameof(Refresh));
        }

        /// <inheritdoc />
        public Task<Result> RemoveAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
        {
            return ExecuteInParallelAsync(caches, (x, ct) => x.RemoveAsync(ct), token);
        }

        /// <inheritdoc />
        public Result Remove<T>(IReadOnlyCachesCollection<T> caches)
        {
            return new NotSupportedSyncStrategyMethodException(nameof(ParallelStrategy), nameof(Remove));
        }

        /// <inheritdoc />
        public Task<Result> SetAsync<T>(IReadOnlyCachesCollection<T> caches, T value,
            CancellationToken token = default)
        {
            return ExecuteInParallelAsync(caches, (x, ct) => x.SetAsync(value, ct), token);
        }

        /// <inheritdoc />
        public Result Set<T>(IReadOnlyCachesCollection<T> caches, T value)
        {
            return new NotSupportedSyncStrategyMethodException(nameof(ParallelStrategy), nameof(Set));
        }

        private static async Task<Result> ExecuteInParallelAsync<T>(IReadOnlyCachesCollection<T> caches,
            Func<IUnknownStoreCache<T>, CancellationToken, ValueTask<Result>> func, CancellationToken token)
        {
            var tasksList = new List<Task<Result>>();
            var count = caches.Count;
            var fails = new List<Result>();
            for (var i = 0; i < count; i++)
            {
                var valueTask = func(caches.ElementAt(i), token);
                if (valueTask.IsCompletedSuccessfully)
                {
                    if (!valueTask.Result.Successful) fails.Add(valueTask.Result);
                    continue;
                }

                tasksList.Add(valueTask.AsTask());
            }

            var results = await Task.WhenAll(tasksList);
            var length = results.Length;
            for (var i = 0; i < length; i++)
            {
                var r = results[i];
                if (!r.Successful) fails.Add(r);
            }

            return fails.Any()
                ? new ParallelStrategyFailException(fails)
                : Result.Success;
        }
    }
}