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
    public sealed class SequenceStrategy : ICachingRemoveStrategy, ICachingSetStrategy, ICachingRefreshStrategy
    {
        private SequenceStrategy()
        {
        }

        public static SequenceStrategy Instance { get; } = new();

        public Task<Result> RemoveAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
            => ExecuteInSequenceAsync(caches, (x, ct) => x.RemoveAsync(ct), token);

        public Result Remove<T>(IReadOnlyCachesCollection<T> caches)
            => ExecuteInSequence(caches, x => x.Remove());

        public Task<Result> SetAsync<T>(IReadOnlyCachesCollection<T> caches, T value, CancellationToken token = default)
            => ExecuteInSequenceAsync(caches, (x, ct) => x.SetAsync(value, ct), token);

        public Result Set<T>(IReadOnlyCachesCollection<T> caches, T value)
            => ExecuteInSequence(caches, x => x.Set(value));

        public Task<Result> RefreshAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
            => ExecuteInSequenceAsync(caches, (x, ct) => x.RefreshAsync(ct), token);

        public Result Refresh<T>(IReadOnlyCachesCollection<T> caches)
            => ExecuteInSequence(caches, x => x.Refresh());
        
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
                    if (!valueTask.Result.Successful)
                    {
                        fails.Add(valueTask.Result);
                    } 
                    continue;
                }

                var awaited = await valueTask;
                if (!awaited.Successful)
                {
                    fails.Add(awaited);
                }
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
                if (!r.Successful)
                {
                    fails.Add(r);
                }
            }

            return fails.Any()
                ? new SequenceStrategyFailException(fails)
                : Result.Success;
        }
    }
}