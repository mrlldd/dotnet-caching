using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Performance
{
    internal class
        PerformanceLoggingCacheStore<TCachingStore, TOptions> : ICacheStore<TOptions>
        where TCachingStore : ICacheStore<TOptions>
    {
        private readonly TCachingStore sourceCacheStore;
        private readonly ILogger<PerformanceLoggingCacheStore<TCachingStore, TOptions>> logger;
        private readonly ICachingPerformanceLoggingOptions performanceLoggingOptions;
        private readonly string storeLogPrefix;

        protected PerformanceLoggingCacheStore(TCachingStore sourceCacheStore,
            ILogger<PerformanceLoggingCacheStore<TCachingStore, TOptions>> logger,
            ICachingPerformanceLoggingOptions performanceLoggingOptions,
            string storeLogPrefix)
        {
            this.sourceCacheStore = sourceCacheStore;
            this.logger = logger;
            this.performanceLoggingOptions = performanceLoggingOptions;
            this.storeLogPrefix = storeLogPrefix;
        }

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
            => ThroughStopwatch((s,m) => s.Get<T>(key, m), metadata);

        public Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => ThroughStopwatchAsync((s, m) => s.GetAsync<T>(key,m, token), metadata);

        public Result Set<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata)
            => ThroughStopwatch((s, m) => s.Set(key, value, options, m), metadata);

        public Task<Result> SetAsync<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => ThroughStopwatchAsync((s, m) => s.SetAsync(key, value, options, m, token), metadata);

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
            => ThroughStopwatch((s, m) => s.Refresh(key, m), metadata);

        public Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
            => ThroughStopwatchAsync((s, m) => s.RefreshAsync(key, m, token), metadata);

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
            => ThroughStopwatch((s, m) => s.Remove(key, m), metadata);

        public Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
            => ThroughStopwatchAsync((s, m) => s.RemoveAsync(key, m, token), metadata);

        private T ThroughStopwatch<T>(Func<TCachingStore, ICacheStoreOperationMetadata, T> func,
            ICacheStoreOperationMetadata metadata)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = func(sourceCacheStore, metadata);
            stopwatch.Stop();
            LogElapsedTime(stopwatch, metadata);
            return result;
        }

        private async Task<T> ThroughStopwatchAsync<T>(Func<TCachingStore, ICacheStoreOperationMetadata, Task<T>> asyncFunc,
            ICacheStoreOperationMetadata metadata)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await asyncFunc(sourceCacheStore, metadata);
            stopwatch.Stop();
            LogElapsedTime(stopwatch, metadata);
            return result;
        }

        private void LogElapsedTime(Stopwatch stopwatch, ICacheStoreOperationMetadata metadata)
            => logger.Log(performanceLoggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Operation executed in {ElapsedMilliseconds}ms.", storeLogPrefix,
                metadata.OperationId,
                stopwatch.Elapsed.TotalMilliseconds);
    }
}