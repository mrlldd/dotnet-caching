﻿using System.Threading;
using DryIoc;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Loaders.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;

namespace mrlldd.Caching.Tests.Caches.TestUtilities.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainer WithNoCaches<T>(this IContainer container)
        {
            container.Register<ICache<T>, BubbleCache<T>>();
            return container;
        }

        public static IContainer MockStores(this IContainer container, MockRepository mockRepository)
        {
            container
                .Resolve<IDistributedCachingStore>()
                .Map(x => MockStore<IDistributedCachingStore, DistributedCacheEntryOptions>(x, mockRepository))
                .AddToContainer(container);
            container
                .Resolve<IMemoryCachingStore>()
                .Map(x => MockStore<IMemoryCachingStore, MemoryCacheEntryOptions>(x, mockRepository))
                .AddToContainer(container);
            return container;
        }

        private static Mock<TStore> MockStore<TStore, TOptions>(TStore store, MockRepository repository)
            where TStore : class, ICachingStore<TOptions> 
            => repository
                .Create<TStore>()
                .Effect(s =>
                    s.Setup(x => x.GetAsync<TestUnit>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                        .Returns<string, CancellationToken>(store.GetAsync<TestUnit>))
                .Effect(s => s.Setup(x => x.SetAsync(It.IsAny<string>(),
                        It.IsAny<TestUnit>(),
                        It.IsAny<TOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<string, TestUnit, TOptions, CancellationToken>(store.SetAsync))
                .Effect(s => s.Setup(x => x.SetAsync(It.IsAny<string>(),
                        It.IsAny<byte[]>(),
                        It.IsAny<TOptions>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<string, byte[], TOptions, CancellationToken>(store.SetAsync))
                .Effect(s => s.Setup(x => x.RemoveAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Returns<string, CancellationToken>(store.RemoveAsync));

        public static IContainer WithMemoryCacheOnly<T>(this IContainer container)
        {
            container.Register<ICache<T>, OnlyMemoryCache<T>>();
            return container;
        }

        public static IContainer WithDistributedCacheOnly<T>(this IContainer container)
        {
            container.Register<ICache<T>, OnlyDistributedCache<T>>();
            return container;
        }
    }
}