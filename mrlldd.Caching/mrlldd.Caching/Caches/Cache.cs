using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Caches
{
    /// <summary>
    /// The base class for implementing caches.
    /// </summary>
    /// <typeparam name="T">The cached objects type.</typeparam>
    public abstract class Cache<T> : Caching<T>, ICache<T>
    {
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        /// <summary>
        /// The default key suffix for given cache type.
        /// </summary>
        protected virtual string DefaultKeySuffix { get; } = typeof(T).ToString();

        /// <inheritdoc />
        public Task SetAsync(T value, CancellationToken token = default)
            => PerformCachingAsync(value, DefaultKeySuffix, token);

        /// <inheritdoc />
        public Task<T> GetAsync(CancellationToken token = default)
            => TryGetFromCacheAsync(DefaultKeySuffix, token);
    }
}