using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal class ActionsLoggingCacheStore<TFlag> : ICacheStore<TFlag> where TFlag : CachingFlag
    {
        private readonly ICacheStore<TFlag> sourceCacheStore;
        private readonly ILogger<ICacheStore<TFlag>> logger;
        private readonly ICachingActionsLoggingOptions loggingOptions;
        private readonly string storeLogPrefix;

        public ActionsLoggingCacheStore(ICacheStore<TFlag> sourceCacheStore,
            ILogger<ICacheStore<TFlag>> logger,
            ICachingActionsLoggingOptions loggingOptions,
            string storeLogPrefix)
        {
            this.sourceCacheStore = sourceCacheStore;
            this.logger = logger;
            this.loggingOptions = loggingOptions;
            this.storeLogPrefix = storeLogPrefix;
        }

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
        {
            LogGetTry<T>(key, metadata);
            return sourceCacheStore.Get<T>(key, metadata)
                .Effect(result => LogGetResult(result, key, metadata));
        }

        public async Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            LogGetTry<T>(key, metadata);
            var result = await sourceCacheStore.GetAsync<T>(key, metadata, token);
            LogGetResult(result, key, metadata);
            return result;
        }

        public Result Set<T>(string key, T value, CachingOptions options, ICacheStoreOperationMetadata metadata)
        {
            LogSetTry<T>(key, metadata);
            var result = sourceCacheStore.Set(key, value, options, metadata);
            LogSetResult<T>(result, key, metadata);
            return result;
        }

        public async Task<Result> SetAsync<T>(string key, T value, CachingOptions options,
            ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            LogSetTry<T>(key, metadata);
            var result = await sourceCacheStore.SetAsync(key, value, options, metadata, token);
            LogSetResult<T>(result, key, metadata);
            return result;
        }

        public Result Refresh(string key, ICacheStoreOperationMetadata metadata)
        {
            LogRefreshTry(key, metadata);
            var result = sourceCacheStore.Refresh(key, metadata);
            LogRefreshResult(result, key, metadata);
            return result;
        }

        public async Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            LogRefreshTry(key, metadata);
            var result = await sourceCacheStore.RefreshAsync(key, metadata, token);
            LogRefreshResult(result, key, metadata);
            return result;
        }

        public Result Remove(string key, ICacheStoreOperationMetadata metadata)
        {
            LogRemoveTry(key, metadata);
            var result = sourceCacheStore.Remove(key, metadata);
            LogRemovingResult(result, key, metadata);
            return result;
        }

        public async Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata,
            CancellationToken token = default)
        {
            LogRemoveTry(key, metadata);
            var result = await sourceCacheStore.RemoveAsync(key, metadata, token);
            LogRemovingResult(result, key, metadata);
            return result;
        }

        private void LogGetTry<T>(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, metadata.OperationId, key, typeof(T).Name);

        private void LogGetResult<T>(Result<T> result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                var unwrapped = result.UnwrapAsSuccess();
                logger.Log(
                    loggingOptions.LogLevel,
                    unwrapped != null
                        ? "[{Store}] [{CacheStoreOperationId:D5}] Successfully got entry with key \"{EntryKey}\" with value of type \"{TypeName}\"."
                        : "[{Store}] [{CacheStoreOperationId:D5}] Not found entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
            else
            {
                logger.Log(loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
        }

        private void LogSetTry<T>(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(
                loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, metadata.OperationId, key, typeof(T).Name);

        private void LogSetResult<T>(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
            else
            {
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
        }

        private void LogRefreshTry(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to refresh entry with key \"{EntryKey}\".",
                storeLogPrefix, metadata.OperationId, key);

        private void LogRefreshResult(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully refreshed entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
            else
            {
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to refresh entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
        }

        private void LogRemoveTry(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to remove entry with key \"{EntryKey}\".",
                storeLogPrefix, metadata.OperationId, key);

        private void LogRemovingResult(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully removed entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
            else
            {
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to remove entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
        }
    }
}