using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Serializers.Internal
{
    internal class CachingSerializerProvider
    {
        public ICachingSerializer Serializer { get; }

        public CachingSerializerProvider(ICachingSerializer serializer)
        {
            Serializer = serializer;
        }
    }
    
    internal class CachingSerializerProvider<TFlag> where TFlag : CachingFlag
    {
        public ICachingSerializer Serializer { get; }

        public CachingSerializerProvider(ICachingSerializer serializer)
        {
            Serializer = serializer;
        }
    }
}