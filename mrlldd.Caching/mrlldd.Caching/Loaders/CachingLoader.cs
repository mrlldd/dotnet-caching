using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Functional.Object.Extensions;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Loaders
{
    public abstract class CachingLoader<TArgs, TResult> : Caching<TResult>, ICachingLoader<TArgs, TResult> where TResult : class
    {
        public async Task<TResult> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default)
        {
            var keySuffix = CacheKeySuffixFactory(args);
            if (!omitCacheOnLoad)
            {
                var inCache = await TryGetFromCacheAsync(keySuffix, token);
                if (inCache != null)
                {
                    return inCache;
                }
            }
            
            var loaded = await LoadAsync(args, token);
            return await loaded.EffectAsync(x => PerformCachingAsync(x, keySuffix, token));
        }

        protected sealed override IEnumerable<string> CacheKeyPrefixesFactory()
            => new List<string>
            {
                "loader"
            };

        protected abstract Task<TResult> LoadAsync(TArgs args, CancellationToken token = default);

        protected abstract string CacheKeySuffixFactory(TArgs args);
    }
}