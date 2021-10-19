using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Loaders.Internal
{
    internal interface IInternalLoader<TArgs, TResult> : ICaching
    {
    }

    internal interface IInternalLoader<TArgs, TResult, TFlag> : IInternalLoader<TArgs, TResult>
        where TFlag : CachingFlag
    {
    }
}