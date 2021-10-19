namespace mrlldd.Caching.Exceptions
{
    /// <summary>
    ///     The exception that represents the missing of loader service required by specific caching loader.
    /// </summary>
    /// <typeparam name="TArgs">The type of arguments.</typeparam>
    /// <typeparam name="TResult">The type of result.</typeparam>
    public class LoaderNotFoundException<TArgs, TResult> : CachingException
    {
        /// <summary>
        ///     The constructor.
        /// </summary>
        public LoaderNotFoundException() : base(
            $"Loader with args '{typeof(TArgs).FullName}' and result '{typeof(TResult).FullName}' has not been found. Seems like it has not been registered.")
        {
        }
    }
}