using System.Collections.Generic;
using mrlldd.Caching.Caches.Internal;

namespace mrlldd.Caching.Caches
{
    internal class Caches<T> : ICaches<T>
    {
        private readonly ICollection<ICache<T>> caches;

        public Caches(ICollection<ICache<T>> caches) 
            => this.caches = caches;
    }
}