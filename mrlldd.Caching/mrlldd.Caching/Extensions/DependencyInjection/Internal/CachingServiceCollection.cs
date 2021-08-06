using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace mrlldd.Caching.Extensions.DependencyInjection.Internal
{
    internal class CachingServiceCollection : ICachingServiceCollection 
    {
        private readonly IServiceCollection serviceCollection;
        public CachingServiceCollection(IServiceCollection serviceCollection) 
            => this.serviceCollection = serviceCollection;

        public IEnumerator<ServiceDescriptor> GetEnumerator()
            => serviceCollection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => serviceCollection.GetEnumerator();

        public void Add(ServiceDescriptor item)
            => serviceCollection.Add(item);

        public void Clear()
            => serviceCollection.Clear();

        public bool Contains(ServiceDescriptor item)
            => serviceCollection.Contains(item);

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => serviceCollection.CopyTo(array, arrayIndex);

        public bool Remove(ServiceDescriptor item)
            => serviceCollection.Remove(item);

        public int Count => serviceCollection.Count;
        public bool IsReadOnly => serviceCollection.IsReadOnly;

        public int IndexOf(ServiceDescriptor item)
            => serviceCollection.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item)
            => serviceCollection.Insert(index, item);

        public void RemoveAt(int index)
            => serviceCollection.RemoveAt(index);

        public ServiceDescriptor this[int index]
        {
            get => serviceCollection[index];
            set => serviceCollection[index] = value;
        }
    }
}