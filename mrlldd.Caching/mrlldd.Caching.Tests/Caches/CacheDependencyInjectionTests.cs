using System;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Internal;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Caches.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    [TestFixture]
    public class CacheDependencyInjectionTests
    {
        private IServiceProvider ServiceProvider { get; set; } = null!;

        private IServiceScope Scope { get; set; } = null!;

        [SetUp]
        public void BuildServicesCollection()
            => new ServiceCollection()
                .AddCaching(typeof(TestCache<>).Assembly)
                .WithActionsLogging()
                .AddLogging(x => x.AddConsole().AddFilter(l => l >= LogLevel.Debug))
                .AddScoped<CacheUsingClass>()
                .BuildServiceProvider()
                .Map(x => Scope = x.CreateScope())
                .Effect(x => ServiceProvider = x.ServiceProvider);

        [TearDown]
        public void DisposeScope()
        {
            Scope.Dispose();
            Scope = null!;
        }

        [Test]
        public void ResolvesOneLayerInheritanceCache()
        {
            var func = new Func<ICache<int>>(() => ServiceProvider.GetRequiredService<ICache<int>>());
            func.Should().NotThrow();
            var cache = func();
            cache.Should().NotBeNull().And.BeOfType<OneLayerInheritanceCache>();
        }

        [Test]
        public void PopulatesWhileResolving()
        {
            var mock = new Mock<ICacheProvider>();
            mock.Setup(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalCacheService<byte>))))
                .Returns(new GenericCacheImplementation2())
                .Verifiable();
            var cache = new ServiceCollection()
                .AddCaching(typeof(GenericCacheImplementation2).Assembly)
                .AddScoped(_ => mock.Object)
                .BuildServiceProvider()
                .GetRequiredService<ICache<byte>>();
            cache.Should().NotBeNull().And.BeOfType<GenericCacheImplementation2>();
            mock.Verify(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalCacheService<byte>))), Times.Once);
        }

        [Test]
        public void ResolvesImplementationOfGenericInheritor()
        {
            var func = new Func<ICache<byte[]>>(() => ServiceProvider.GetRequiredService<ICache<byte[]>>());
            func.Should().NotThrow();
            var cache = func();
            cache.Should().BeOfType<GenericCacheImplementationChild>();
            var setAction = new Action(() => cache.Set(Array.Empty<byte>()));
            setAction.Should().NotThrow();
            var getFunc = new Func<byte[]>(() => cache.Get()!);
            getFunc.Should().NotThrow();
        }

        [Test]
        public void PopulatesAndResolvesUserClassWithCacheDependency()
        {
            var mock = new Mock<ICacheProvider>();
            mock.Setup(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalCacheService<byte[]>))))
                .Returns(new GenericCacheImplementationChild())
                .Verifiable();
            var provider = new ServiceCollection()
                .AddCaching(typeof(GenericCacheImplementation2).Assembly)
                .AddScoped(_ => mock.Object)
                .AddScoped<CacheUsingClass>()
                .BuildServiceProvider();
            var func = new Func<CacheUsingClass>(() => provider.GetRequiredService<CacheUsingClass>());
            func.Should().NotThrow();
            func().DataCache
                .Should()
                .NotBeNull()
                .And.BeOfType<GenericCacheImplementationChild>();
            mock.Verify(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalCacheService<byte[]>))), Times.Once);
        }

        [Test]
        public void ResolvedCacheUsesActualStores()
        {
            var memoryStoreMock = new Mock<IMemoryCacheStore>();
            var distributedStoreMock = new Mock<IDistributedCacheStore>();
            var provider = new ServiceCollection()
                .AddCaching(typeof(GenericCacheImplementation2).Assembly)
                .AddScoped(_ => memoryStoreMock.Object)
                .AddScoped(_ => distributedStoreMock.Object)
                .BuildServiceProvider();
            var cache = provider.GetRequiredService<ICache<int>>();
            cache.Set(0);
            memoryStoreMock
                .Verify(
                    x => x.Set("OneLayer:test", It.IsAny<int>(), It.IsAny<MemoryCacheEntryOptions>(),
                        It.IsAny<ICacheStoreOperationMetadata>()), Times.Once);
            distributedStoreMock
                .Verify(
                    x => x.Set("OneLayer:test", It.IsAny<int>(), It.IsAny<DistributedCacheEntryOptions>(),
                        It.IsAny<ICacheStoreOperationMetadata>()), Times.Once);
        }


        public class TwoLayerInheritanceCache : TestCache<string>
        {
        }

        public class OneLayerInheritanceCache : Cache<int>
        {
            protected override CachingOptions MemoryCacheOptions =>
                CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));

            protected override CachingOptions DistributedCacheOptions =>
                CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));

            protected override string CacheKey => "OneLayer";
            protected override string DefaultKeySuffix => "test";
        }

        public class GenericCache<T> : Cache<T>
        {
            protected override CachingOptions MemoryCacheOptions =>
                CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));

            protected override CachingOptions DistributedCacheOptions =>
                CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));

            protected override string CacheKey => nameof(T);
        }

        public class GenericCacheImplementation2 : GenericCache<byte>
        {
        }

        public class GenericCacheImplementation : GenericCache<byte[]>
        {
        }

        public class GenericCacheImplementationChild : GenericCacheImplementation
        {
        }

        public class CacheUsingClass
        {
            public readonly ICache<byte[]> DataCache;

            public CacheUsingClass(ICache<byte[]> dataCache)
                => DataCache = dataCache;
        }
    }
}