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
    ///     The class that represents a racing get strategy.
    /// </summary>
    public sealed class GetFirstSuccessfulStrategy : ICacheGetStrategy
    {
        private GetFirstSuccessfulStrategy()
        {
        }

        /// <summary>
        ///     The singleton instance.
        /// </summary>
        public static ICacheGetStrategy Instance { get; } = new GetFirstSuccessfulStrategy();

        /// <inheritdoc />
        public async Task<Result<T?>> GetAsync<T>(IReadOnlyCachesCollection<T> caches, CancellationToken token = default)
        {
            var tasksList = new List<Task<Result<T?>>>();
            var count = caches.Count;
            var fails = new List<Result<T?>>();
            for (var i = 0; i < count; i++)
            {
                var valueTask = caches.ElementAt(i).GetAsync(token);
                if (valueTask.IsCompletedSuccessfully)
                {
                    if (valueTask.Result.Successful) return valueTask.Result;
                    fails.Add(valueTask.Result);
                    continue;
                }

                tasksList.Add(valueTask.AsTask());
            }

            var task = await Task.WhenAny(tasksList);
            if (task.Result.Successful) return task.Result;

            while (tasksList.Any())
            {
                tasksList.Remove(task);
                task = await Task.WhenAny(tasksList);
                if (task.Result.Successful) return task.Result;
                fails.Add(task.Result);
            }

            return task.Result.Successful
                ? task.Result
                : new StrategyMissException<T>(nameof(GetFirstSuccessfulStrategy), fails);
        }

        /// <inheritdoc />
        public Result<T?> Get<T>(IReadOnlyCachesCollection<T> caches)
        {
            var count = caches.Count;
            var fails = new List<Result<T?>>();
            for (var i = 0; i < count; i++)
            {
                var result = caches.ElementAt(i).Get();
                if (result.Successful) return result;
                fails.Add(result);
            }

            return new StrategyMissException<T>(nameof(GetFirstSuccessfulStrategy), fails);
        }
    }
}