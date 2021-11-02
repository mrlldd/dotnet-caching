using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal class PerformanceLoggingCacheStore<TFlag> : ICacheStore<TFlag> where TFlag : CachingFlag
    {
        private readonly ILogger<ICacheStore<TFlag>> logger;
        private readonly ICachingPerformanceLoggingOptions loggingOptions;
        private readonly ICacheStore<TFlag> sourceCacheStore;
        private readonly string storeLogPrefix;

        public PerformanceLoggingCacheStore(ICacheStore<TFlag> sourceCacheStore,
            ILogger<ICacheStore<TFlag>> logger,
            ICachingPerformanceLoggingOptions loggingOptions,
            string storeLogPrefix)
        {
            this.sourceCacheStore = sourceCacheStore;
            this.logger = logger;
            this.loggingOptions = loggingOptions;
            this.storeLogPrefix = storeLogPrefix;
        }

        public Result<T?> Get<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            return ThroughStopwatch((s, m) => s.Get<T>(key, m), operationOptions);
        }

        public ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.GetAsync<T>(key, m, token), operationOptions);
        }

        public Result Set<T>(string key, T? value, CachingOptions options, ICacheStoreOperationOptions operationOptions)
        {
            return ThroughStopwatch((s, m) => s.Set(key, value, options, m), operationOptions);
        }

        public ValueTask<Result> SetAsync<T>(string key, T? value, CachingOptions options,
            ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.SetAsync(key, value, options, m, token), operationOptions);
        }

        public Result Refresh(string key, ICacheStoreOperationOptions operationOptions)
        {
            return ThroughStopwatch((s, m) => s.Refresh(key, m), operationOptions);
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.RefreshAsync(key, m, token), operationOptions);
        }

        public Result Remove(string key, ICacheStoreOperationOptions operationOptions)
        {
            return ThroughStopwatch((s, m) => s.Remove(key, m), operationOptions);
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.RemoveAsync(key, m, token), operationOptions);
        }

        private T ThroughStopwatch<T>(Func<ICacheStore<TFlag>, ICacheStoreOperationOptions, T> func,
            ICacheStoreOperationOptions operationOptions)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = func(sourceCacheStore, operationOptions);
            stopwatch.Stop();
            LogElapsedTime(stopwatch, operationOptions);
            return result;
        }

        private async ValueTask<T> ThroughStopwatchAsync<T>(
            Func<ICacheStore<TFlag>, ICacheStoreOperationOptions, ValueTask<T>> asyncFunc,
            ICacheStoreOperationOptions operationOptions)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var task = asyncFunc(sourceCacheStore, operationOptions);
            var result = task.IsCompletedSuccessfully
                ? task.Result
                : await task;
            stopwatch.Stop();
            LogElapsedTime(stopwatch, operationOptions);
            return result;
        }

        private void LogElapsedTime(Stopwatch stopwatch, ICacheStoreOperationOptions operationOptions)
        {
            logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Action executed in {ElapsedMilliseconds}ms.", storeLogPrefix,
                operationOptions.OperationId,
                stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}