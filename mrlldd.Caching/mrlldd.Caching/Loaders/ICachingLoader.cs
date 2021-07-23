using System.Threading;
using System.Threading.Tasks;
using mrlldd.Caching.Caching;

namespace mrlldd.Caching.Loaders
{
    public interface ICachingLoader<in TArgs, TResult> : ICaching<TResult>
    {
        Task<TResult> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);
    }
}