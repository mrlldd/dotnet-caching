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

        public Result<T> Get<T>(string key, ICacheStoreOperationMetadata metadata)
        {
            return ThroughStopwatch((s, m) => s.Get<T>(key, m), metadata);
        }

        public ValueTask<Result<T>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.GetAsync<T>(key, m, token), metadata);
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
        {
            return ThroughStopwatch((s, m) => s.Set(key, value, options, m), metadata);
        }

        public ValueTask<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.SetAsync(key, value, options, m, token), metadata);
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
        {
            return ThroughStopwatch((s, m) => s.Refresh(key, m), metadata);
        }

        public ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.RefreshAsync(key, m, token), metadata);
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
        {
            return ThroughStopwatch((s, m) => s.Remove(key, m), metadata);
        }

        public ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            return ThroughStopwatchAsync((s, m) => s.RemoveAsync(key, m, token), metadata);
        }

        private T ThroughStopwatch<T>(Func<ICacheStore<TFlag>, ICacheStoreOperationMetadata, T> func,
            ICacheStoreOperationMetadata metadata)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = func(sourceCacheStore, metadata);
            stopwatch.Stop();
            LogElapsedTime(stopwatch, metadata);
            return result;
        }

        private async ValueTask<T> ThroughStopwatchAsync<T>(
            Func<ICacheStore<TFlag>, ICacheStoreOperationMetadata, ValueTask<T>> asyncFunc,
            ICacheStoreOperationMetadata metadata)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var task = asyncFunc(sourceCacheStore, metadata);
            var result = task.IsCompletedSuccessfully
                ? task.Result
                : await task;
            stopwatch.Stop();
            LogElapsedTime(stopwatch, metadata);
            return result;
        }

        private void LogElapsedTime(Stopwatch stopwatch, ICacheStoreOperationMetadata metadata)
        {
            logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Action executed in {ElapsedMilliseconds}ms.", storeLogPrefix,
                metadata.OperationId,
                stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}