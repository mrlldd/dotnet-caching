using System.Threading;
using System.Threading.Tasks;
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
        private readonly ILogger<ICacheStore<TFlag>> logger;
        private readonly ICachingActionsLoggingOptions loggingOptions;
        private readonly ICacheStore<TFlag> sourceCacheStore;
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

        public Result<T?> Get<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            LogGetTry<T>(key, operationOptions);
            var result = sourceCacheStore.Get<T>(key, operationOptions);
            LogGetResult(result, key, operationOptions);
            return result;
        }

        public async ValueTask<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            LogGetTry<T>(key, operationOptions);
            var result = await sourceCacheStore.GetAsync<T>(key, operationOptions, token);
            LogGetResult(result, key, operationOptions);
            return result;
        }

        public Result Set<T>(string key, T? value, CachingOptions options, ICacheStoreOperationOptions operationOptions)
        {
            LogSetTry<T>(key, operationOptions);
            var result = sourceCacheStore.Set(key, value, options, operationOptions);
            LogSetResult<T>(result, key, operationOptions);
            return result;
        }

        public async ValueTask<Result> SetAsync<T>(string key, T? value, CachingOptions options,
            ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            LogSetTry<T>(key, operationOptions);
            var result = await sourceCacheStore.SetAsync(key, value, options, operationOptions, token);
            LogSetResult<T>(result, key, operationOptions);
            return result;
        }

        public Result Refresh(string key, ICacheStoreOperationOptions operationOptions)
        {
            LogRefreshTry(key, operationOptions);
            var result = sourceCacheStore.Refresh(key, operationOptions);
            LogRefreshResult(result, key, operationOptions);
            return result;
        }

        public async ValueTask<Result> RefreshAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            LogRefreshTry(key, operationOptions);
            var result = await sourceCacheStore.RefreshAsync(key, operationOptions, token);
            LogRefreshResult(result, key, operationOptions);
            return result;
        }

        public Result Remove(string key, ICacheStoreOperationOptions operationOptions)
        {
            LogRemoveTry(key, operationOptions);
            var result = sourceCacheStore.Remove(key, operationOptions);
            LogRemovingResult(result, key, operationOptions);
            return result;
        }

        public async ValueTask<Result> RemoveAsync(string key, ICacheStoreOperationOptions operationOptions,
            CancellationToken token = default)
        {
            LogRemoveTry(key, operationOptions);
            var result = await sourceCacheStore.RemoveAsync(key, operationOptions, token);
            LogRemovingResult(result, key, operationOptions);
            return result;
        }

        private void LogGetTry<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
        }

        private void LogGetResult<T>(Result<T> result, string key, ICacheStoreOperationOptions operationOptions)
        {
            if (result.Successful)
            {
                var unwrapped = result.UnwrapAsSuccess();
                logger.Log(
                    loggingOptions.LogLevel,
                    unwrapped != null
                        ? "[{Store}] [{CacheStoreOperationId:D5}] Successfully got entry with key \"{EntryKey}\" with value of type \"{TypeName}\"."
                        : "[{Store}] [{CacheStoreOperationId:D5}] Not found entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
            }
            else
            {
                logger.Log(loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
            }
        }

        private void LogSetTry<T>(string key, ICacheStoreOperationOptions operationOptions)
        {
            logger.Log(
                loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
        }

        private void LogSetResult<T>(Result result, string key, ICacheStoreOperationOptions operationOptions)
        {
            if (result.Successful)
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
            else
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, operationOptions.OperationId, key, typeof(T).Name);
        }

        private void LogRefreshTry(string key, ICacheStoreOperationOptions operationOptions)
        {
            logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to refresh entry with key \"{EntryKey}\".",
                storeLogPrefix, operationOptions.OperationId, key);
        }

        private void LogRefreshResult(Result result, string key, ICacheStoreOperationOptions operationOptions)
        {
            if (result.Successful)
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully refreshed entry with key \"{EntryKey}\".",
                    storeLogPrefix, operationOptions.OperationId, key);
            else
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to refresh entry with key \"{EntryKey}\".",
                    storeLogPrefix, operationOptions.OperationId, key);
        }

        private void LogRemoveTry(string key, ICacheStoreOperationOptions operationOptions)
        {
            logger.Log(loggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to remove entry with key \"{EntryKey}\".",
                storeLogPrefix, operationOptions.OperationId, key);
        }

        private void LogRemovingResult(Result result, string key, ICacheStoreOperationOptions operationOptions)
        {
            if (result.Successful)
                logger.Log(
                    loggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully removed entry with key \"{EntryKey}\".",
                    storeLogPrefix, operationOptions.OperationId, key);
            else
                logger.Log(
                    loggingOptions.ErrorsLogLevel,
                    result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to remove entry with key \"{EntryKey}\".",
                    storeLogPrefix, operationOptions.OperationId, key);
        }
    }
}