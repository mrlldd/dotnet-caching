namespace mrlldd.Caching.Exceptions
{
    public class LoaderNotFoundException<TArgs, TResult> : CachingException
    {
        public LoaderNotFoundException() : base($"Loader with args '{typeof(TArgs).FullName}' and result '{typeof(TResult).FullName}' has not been found. Seems like it has not been registered.")
        {
        }
    }
}