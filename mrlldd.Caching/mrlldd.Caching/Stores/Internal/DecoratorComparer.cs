using System.Collections.Generic;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores.Decoration;

namespace mrlldd.Caching.Stores.Internal
{
    internal class DecoratorComparer<T> : IComparer<ICacheStoreDecorator<T>> where T : CachingFlag
    {
        private DecoratorComparer()
        {
        }

        public static DecoratorComparer<T> Instance { get; } = new();
        public int Compare(ICacheStoreDecorator<T> x, ICacheStoreDecorator<T> y) 
            => ReferenceEquals(x, y)
                ? 0
                : x.Order.CompareTo(y.Order);
    }
}