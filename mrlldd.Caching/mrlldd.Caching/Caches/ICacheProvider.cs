
namespace mrlldd.Caching.Caches
{
    public interface ICacheProvider
    {
        ICache<T> Get<T>();
    }
}