using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Extensions.DependencyInjection;

namespace mrlldd.Caching.Stores.Decoration.Internal.Logging
{
    internal class LoggingCachingStore<TCachingStore, TOptions> : ICachingStore<TOptions>
        where TCachingStore : ICachingStore<TOptions>
    {
        private readonly TCachingStore sourceCachingStore;
        private readonly ILogger<LoggingCachingStore<TCachingStore, TOptions>> logger;
        private readonly ICachingLoggingOptions loggingOptions;
        private readonly string storeLogPrefix;

        protected LoggingCachingStore(TCachingStore sourceCachingStore,
            ILogger<LoggingCachingStore<TCachingStore, TOptions>> logger,
            ICachingLoggingOptions loggingOptions,
            string storeLogPrefix)
        {
            this.sourceCachingStore = sourceCachingStore;
            this.logger = logger;
            this.loggingOptions = loggingOptions;
            this.storeLogPrefix = storeLogPrefix;
        }

        public Result<T?> Get<T>(string key)
        {
            LogGetTry<T>(key);
            var stopwatch = new Stopwatch();
            return sourceCachingStore.Get<T>(key)
                .Effect(result =>
                {
                    stopwatch.Stop();
                    LogGetResult(result, key, stopwatch);
                });
        }

        public async Task<Result<T?>> GetAsync<T>(string key, CancellationToken token = default)
        {
            LogGetTry<T>(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await sourceCachingStore.GetAsync<T>(key, token);
            stopwatch.Stop();
            LogGetResult(result, key, stopwatch);

            return result;
        }

        public Result Set<T>(string key, T value, TOptions options)
        {
            LogSetTry<T>(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = sourceCachingStore.Set(key, value, options);
            stopwatch.Stop();
            LogSetResult<T>(result, key, stopwatch);
            return result;
        }

        public async Task<Result> SetAsync<T>(string key, T value, TOptions options, CancellationToken token = default)
        {
            LogSetTry<T>(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await sourceCachingStore.SetAsync(key, value, options, token);
            stopwatch.Stop();
            LogSetResult<T>(result, key, stopwatch);
            return result;
        }

        public Result Refresh(string key)
        {
            LogRefreshTry(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = sourceCachingStore.Refresh(key);
            stopwatch.Stop();
            LogRefreshResult(result, key, stopwatch);
            return result;
        }

        public async Task<Result> RefreshAsync(string key, CancellationToken token = default)
        {
            LogRefreshTry(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await sourceCachingStore.RefreshAsync(key, token);
            stopwatch.Stop();
            LogRefreshResult(result, key, stopwatch);
            return result;
        }

        public Result Remove(string key)
        {
            LogRemoveTry(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = sourceCachingStore.Remove(key);
            stopwatch.Stop();
            LogRemovingResult(result, key, stopwatch);
            return result;
        }

        public async Task<Result> RemoveAsync(string key, CancellationToken token = default)
        {
            LogRemoveTry(key);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await sourceCachingStore.RemoveAsync(key, token);
            stopwatch.Stop();
            LogRemovingResult(result, key, stopwatch);
            return result;
        }

        private void LogGetTry<T>(string key)
            => logger.Log(loggingOptions.LogLevel,
                "[{Store}] Trying to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, key, typeof(T).Name);

        private void LogGetResult<T>(Result<T> result, string key, Stopwatch stopwatch)
        {
            var totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            if (result.Successful)
            {
                var unwrapped = result.UnwrapAsSuccess();
                logger.Log(
                    loggingOptions.LogLevel,
                    unwrapped != null
                        ? "[{Store}] Successfully got entry with key \"{EntryKey}\" with value of type \"{TypeName}\" in {ElapsedMilliseconds}ms."
                        : "[{Store}] Not found entry with key \"{EntryKey}\" with value of type \"{TypeName}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, typeof(T).Name, totalMilliseconds);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] Failed to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, typeof(T).Name, totalMilliseconds);
            }
        }

        private void LogSetTry<T>(string key)
            => logger.Log(
                loggingOptions.LogLevel,
                "[{Store}] Trying to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, key, typeof(T).Name);

        private void LogSetResult<T>(Result result, string key, Stopwatch stopwatch)
        {
            var totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] Successfully set entry with key \"{EntryKey}\" with value of type \"{TypeName}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, typeof(T).Name, totalMilliseconds);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] Failed to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, typeof(T).Name, totalMilliseconds);
            }
        }

        private void LogRefreshTry(string key)
            => logger.Log(loggingOptions.LogLevel,"[{Store}] Trying to refresh entry with key \"{EntryKey}\".", storeLogPrefix, key);

        private void LogRefreshResult(Result result, string key, Stopwatch stopwatch)
        {
            var totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] Successfully refreshed entry with key \"{EntryKey}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, totalMilliseconds);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] Failed to refresh entry with key \"{EntryKey}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, totalMilliseconds);
            }
        }

        private void LogRemoveTry(string key)
            => logger.Log(loggingOptions.LogLevel,"[{Store}] Trying to remove entry with key \"{EntryKey}\".", storeLogPrefix, key);

        private void LogRemovingResult(Result result, string key, Stopwatch stopwatch)
        {
            var totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] Successfully removed entry with key \"{EntryKey}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, totalMilliseconds);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] Failed to remove entry with key \"{EntryKey}\" in {ElapsedMilliseconds}ms.",
                    storeLogPrefix, key, totalMilliseconds);
            }
        }
    }
}