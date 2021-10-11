using System.Threading;
using System.Threading.Tasks;
using Functional.Result;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Loaders.Internal
{
    internal interface IInternalLoader<TArgs, TResult> : ICaching
    {
        ValueTask<Result<TResult>> GetOrLoadAsync(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        Result<TResult> GetOrLoad(TArgs args, bool omitCacheOnLoad = false, CancellationToken token = default);

        ValueTask<Result> SetAsync(TArgs args, TResult result, CancellationToken token = default);

        Result Set(TArgs args, TResult result);
        
        ValueTask<Result<TResult>> GetAsync(TArgs args, CancellationToken token = default);

        Result<TResult> Get(TArgs args);

        ValueTask<Result> RefreshAsync(TArgs args, CancellationToken token = default);

        Result Refresh(TArgs args);

        ValueTask<Result> RemoveAsync(TArgs args, CancellationToken token = default);

        Result Remove(TArgs args);
    }
    
    internal interface IInternalLoader<TArgs, TResult, TFlag> : IInternalLoader<TArgs, TResult> where TFlag : CachingFlag
    {
    }
}