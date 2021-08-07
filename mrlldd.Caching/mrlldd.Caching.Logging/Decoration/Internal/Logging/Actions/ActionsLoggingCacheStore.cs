﻿using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.Logging;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Stores;

namespace mrlldd.Caching.Decoration.Internal.Logging.Actions
{
    internal class ActionsLoggingCacheStore<TCachingStore, TOptions> : ICacheStore<TOptions>
        where TCachingStore : ICacheStore<TOptions>
    {
        private readonly TCachingStore sourceCacheStore;
        private readonly ILogger<ActionsLoggingCacheStore<TCachingStore, TOptions>> logger;
        private readonly ICachingActionsLoggingOptions performanceLoggingOptions;
        private readonly string storeLogPrefix;

        protected ActionsLoggingCacheStore(TCachingStore sourceCacheStore,
            ILogger<ActionsLoggingCacheStore<TCachingStore, TOptions>> logger,
            ICachingActionsLoggingOptions performanceLoggingOptions,
            string storeLogPrefix)
        {
            this.sourceCacheStore = sourceCacheStore;
            this.logger = logger;
            this.performanceLoggingOptions = performanceLoggingOptions;
            this.storeLogPrefix = storeLogPrefix;
        }

        public Result<T?> Get<T>(string key, ICacheStoreOperationMetadata metadata)
        {
            LogGetTry<T>(key, metadata);
            return sourceCacheStore.Get<T>(key, metadata)
                .Effect(result =>
                {
                    LogGetResult(result, key, metadata);
                });
        }

        public async Task<Result<T?>> GetAsync<T>(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
        {
            LogGetTry<T>(key, metadata);
            var result = await sourceCacheStore.GetAsync<T>(key, metadata, token);
            LogGetResult(result, key, metadata);
            return result;
        }

        public Result Set<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata)
        {
            LogSetTry<T>(key, metadata);
            var result = sourceCacheStore.Set(key, value, options, metadata);
            LogSetResult<T>(result, key, metadata);
            return result;
        }

        public async Task<Result> SetAsync<T>(string key, T value, TOptions options, ICacheStoreOperationMetadata metadata,
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

        public async Task<Result> RefreshAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
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

        public async Task<Result> RemoveAsync(string key, ICacheStoreOperationMetadata metadata, CancellationToken token = default)
        {
            LogRemoveTry(key, metadata);
            var result = await sourceCacheStore.RemoveAsync(key, metadata, token);
            LogRemovingResult(result, key, metadata);
            return result;
        }

        private void LogGetTry<T>(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(performanceLoggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, metadata.OperationId, key, typeof(T).Name);

        private void LogGetResult<T>(Result<T> result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                var unwrapped = result.UnwrapAsSuccess();
                logger.Log(
                    performanceLoggingOptions.LogLevel,
                    unwrapped != null
                        ? "[{Store}] [{CacheStoreOperationId:D5}] Successfully got entry with key \"{EntryKey}\" with value of type \"{TypeName}\"."
                        : "[{Store}] [{CacheStoreOperationId:D5}] Not found entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to get entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
        }

        private void LogSetTry<T>(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(
                performanceLoggingOptions.LogLevel,
                "[{Store}] [{CacheStoreOperationId:D5}] Trying to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                storeLogPrefix, metadata.OperationId, key, typeof(T).Name);

        private void LogSetResult<T>(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    performanceLoggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to set entry with key \"{EntryKey}\" with value of type \"{TypeName}\".",
                    storeLogPrefix, metadata.OperationId, key, typeof(T).Name);
            }
        }

        private void LogRefreshTry(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(performanceLoggingOptions.LogLevel, "[{Store}] [{CacheStoreOperationId:D5}] Trying to refresh entry with key \"{EntryKey}\".",
                storeLogPrefix, metadata.OperationId, key);

        private void LogRefreshResult(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    performanceLoggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully refreshed entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to refresh entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
        }

        private void LogRemoveTry(string key, ICacheStoreOperationMetadata metadata)
            => logger.Log(performanceLoggingOptions.LogLevel, "[{Store}] [{CacheStoreOperationId:D5}] Trying to remove entry with key \"{EntryKey}\".",
                storeLogPrefix, metadata.OperationId, key);

        private void LogRemovingResult(Result result, string key, ICacheStoreOperationMetadata metadata)
        {
            if (result.Successful)
            {
                logger.Log(
                    performanceLoggingOptions.LogLevel,
                    "[{Store}] [{CacheStoreOperationId:D5}] Successfully removed entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
            else
            {
                logger.LogError(result,
                    "[{Store}] [{CacheStoreOperationId:D5}] Failed to remove entry with key \"{EntryKey}\".",
                    storeLogPrefix, metadata.OperationId, key);
            }
        }
    }
}