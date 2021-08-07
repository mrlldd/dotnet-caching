namespace mrlldd.Caching.Stores
{
    public interface IStoreOperationProvider
    {
        ICacheStoreOperationMetadata Next();
    }
}