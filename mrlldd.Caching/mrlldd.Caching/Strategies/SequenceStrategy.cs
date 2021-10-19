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
    ///     The class that represents strategy that interacts with caches in sequence.
    /// </summary>
    public sealed class SequenceStrategy : ICachingRemoveStrategy, ICachingSetStrategy, ICachingRefreshStrategy
    {
        private SequenceStrategy()
        {
        }

        /// <summary>
        ///     The singleton instance.
        /// </summary>
        public static SequenceStrategy Instance { get; } = new();

        /// <inheritdoc />
        public Task<Result> RefreshAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
        {
            return ExecuteInSequenceAsync(caches, (x, ct) => x.RefreshAsync(ct), token);
        }

        /// <inheritdoc />
        public Result Refresh<T>(IReadOnlyCachesCollection<T> caches)
        {
            return ExecuteInSequence(caches, x => x.Refresh());
        }

        /// <inheritdoc />
        public Task<Result> RemoveAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
        {
            return ExecuteInSequenceAsync(caches, (x, ct) => x.RemoveAsync(ct), token);
        }

        /// <inheritdoc />
        public Result Remove<T>(IReadOnlyCachesCollection<T> caches)
        {
            return ExecuteInSequence(caches, x => x.Remove());
        }

        /// <inheritdoc />
        public Task<Result> SetAsync<T>(IReadOnlyCachesCollection<T> caches, T value, CancellationToken token = default)
        {
            return ExecuteInSequenceAsync(caches, (x, ct) => x.SetAsync(value, ct), token);
        }

        /// <inheritdoc />
        public Result Set<T>(IReadOnlyCachesCollection<T> caches, T value)
        {
            return ExecuteInSequence(caches, x => x.Set(value));
        }

        private static async Task<Result> ExecuteInSequenceAsync<T>(IReadOnlyCachesCollection<T> caches,
            Func<IUnknownStoreCache<T>, CancellationToken, ValueTask<Result>> func, CancellationToken token)
        {
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

                var awaited = await valueTask;
                if (!awaited.Successful) fails.Add(awaited);
            }

            return fails.Any()
                ? new SequenceStrategyFailException(fails)
                : Result.Success;
        }

        private static Result ExecuteInSequence<T>(IReadOnlyCachesCollection<T> caches,
            Func<IUnknownStoreCache<T>, Result> func)
        {
            var count = caches.Count;
            var fails = new List<Result>();
            for (var i = 0; i < count; i++)
            {
                var r = func(caches.ElementAt(i));
                if (!r.Successful) fails.Add(r);
            }

            return fails.Any()
                ? new SequenceStrategyFailException(fails)
                : Result.Success;
        }
    }
}