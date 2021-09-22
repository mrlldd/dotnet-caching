using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Exceptions
{
    public class StoreNotFoundException<TFlag> : CachingException where TFlag : CachingFlag
    {
        public StoreNotFoundException() : base($"Store for flag '{typeof(TFlag).FullName}' has not been found. Seems like it has not been registered.")
        {
        }
    }
}