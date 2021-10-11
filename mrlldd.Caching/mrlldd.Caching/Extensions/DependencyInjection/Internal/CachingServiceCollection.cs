using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;

namespace mrlldd.Caching.Extensions.DependencyInjection.Internal
{
    internal class CachingServiceCollection : ICachingServiceCollection 
    {
        protected readonly IServiceCollection Services;
        public CachingServiceCollection(IServiceCollection services) 
            => this.Services = services;

        public IEnumerator<ServiceDescriptor> GetEnumerator()
            => Services.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Services.GetEnumerator();

        public void Add(ServiceDescriptor item)
            => Services.Add(item);

        public void Clear()
            => Services.Clear();

        public bool Contains(ServiceDescriptor item)
            => Services.Contains(item);

        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => Services.CopyTo(array, arrayIndex);

        public bool Remove(ServiceDescriptor item)
            => Services.Remove(item);

        public int Count => Services.Count;
        public bool IsReadOnly => Services.IsReadOnly;

        public int IndexOf(ServiceDescriptor item)
            => Services.IndexOf(item);

        public void Insert(int index, ServiceDescriptor item)
            => Services.Insert(index, item);

        public void RemoveAt(int index)
            => Services.RemoveAt(index);

        public ServiceDescriptor this[int index]
        {
            get => Services[index];
            set => Services[index] = value;
        }

        public IDecoratorsCachingServiceCollection<TFlag> Decorators<TFlag>() where TFlag : CachingFlag
            => new DecoratorsCachingServiceCollection<TFlag>(this);
    }
}