using Functional.Result;

namespace mrlldd.Caching.Loaders
{
    /// <summary>
    /// The service used for providing loaders.
    /// </summary>
    public interface ILoaderProvider
    {
 
        /// <summary>
        /// The method used to get a loader.
        /// </summary>
        /// <typeparam name="TArgs">The type of loader argument.</typeparam>
        /// <typeparam name="TResult">The type of loader result.</typeparam>
        /// <returns>The result of getting the caching loader.</returns>
        Result<ICachingLoader<TArgs, TResult>> GetRequired<TArgs, TResult>()
            where TResult : class;
    }
}