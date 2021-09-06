using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Internal;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Loaders
{
    [TestFixture]
    public class CachingLoaderDependencyInjectionTests
    {
        private IServiceProvider ServiceProvider { get; set; } = null!;

        private IServiceScope? Scope { get; set; }

        [SetUp]
        public void BuildServiceCollection()
            => new ServiceCollection()
                .AddCaching(typeof(CachingLoaderDependencyInjectionTests).Assembly)
                .WithActionsLogging()
                .BuildServiceProvider()
                .Map(x => Scope = x.CreateScope())
                .Effect(x => ServiceProvider = x.ServiceProvider);

        [TearDown]
        public void DisposeScope()
        {
            Scope!.Dispose();
            Scope = null;
        }
        
        [Test]
        public void ResolvesOneLayerInheritanceLoader()
        {
            var func = new Func<ICachingLoader<int, string>>(() => ServiceProvider.GetRequiredService<ICachingLoader<int, string>>());
            func.Should().NotThrow();
            var cache = func();
            cache.Should().NotBeNull().And.BeOfType<OneLayerInheritanceCachingLoader>();
        }
        
        [Test]
        public void PopulatesWhileResolving()
        {
            var mock = new Mock<ILoaderProvider>();
            mock.Setup(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalLoaderService<int, string>))))
                .Returns(new OneLayerInheritanceCachingLoader())
                .Verifiable();
            var loader = new ServiceCollection()
                .AddCaching(typeof(OneLayerInheritanceCachingLoader).Assembly)
                .AddScoped(_ => mock.Object)
                .BuildServiceProvider()
                .GetRequiredService<ICachingLoader<int, string>>();
            loader.Should().NotBeNull().And.BeOfType<OneLayerInheritanceCachingLoader>();
            mock.Verify(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalLoaderService<int, string>))), Times.Once);
        }
        
        [Test]
        public void ResolvesImplementationOfGenericInheritor()
        {
            var func = new Func<ICachingLoader<int, TestUnit>>(() => ServiceProvider.GetRequiredService<ICachingLoader<int, TestUnit>>());
            func.Should().NotThrow();
            var loader = func();
            loader.Should().BeOfType<ImplementationGenericLoader>();
            var setAction = new Action(() => loader.Set(0, TestUnit.Create()));
            setAction.Should().NotThrow();
            var getFunc = new Func<TestUnit?>(() => loader.GetOrLoad(0));
            getFunc.Should().NotThrow();
        }
        
        [Test]
        public void PopulatesAndResolvesUserClassWithLoaderDependency()
        {
            var mock = new Mock<ILoaderProvider>();
            mock.Setup(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalLoaderService<int, TestUnit>))))
                .Returns(new ImplementationGenericLoader())
                .Verifiable();
            var provider = new ServiceCollection()
                .AddCaching(typeof(ImplementationGenericLoader).Assembly)
                .AddScoped(_ => mock.Object)
                .AddScoped<LoaderUsingClass>()
                .BuildServiceProvider();
            var func = new Func<LoaderUsingClass>(() => provider.GetRequiredService<LoaderUsingClass>());
            func.Should().NotThrow();
            func().Loader
                .Should()
                .NotBeNull()
                .And.BeOfType<ImplementationGenericLoader>();
            mock.Verify(x => x.GetRequired(It.Is<Type>(t => t == typeof(IInternalLoaderService<int, TestUnit>))), Times.Once);
        }
        
        [Test]
        public void ResolvedCacheUsesActualStores()
        {
            var memoryStoreMock = new Mock<IMemoryCacheStore>();
            var distributedStoreMock = new Mock<IDistributedCacheStore>();
            var provider = new ServiceCollection()
                .AddCaching(typeof(ImplementationGenericLoader).Assembly)
                .AddScoped(_ => memoryStoreMock.Object)
                .AddScoped(_ => distributedStoreMock.Object)
                .BuildServiceProvider();
            var cache = provider.GetRequiredService<ICachingLoader<int, TestUnit>>();
            cache.Set(0, TestUnit.Create());
            memoryStoreMock
                .Verify(
                    x => x.Set("loader:Generic:0", It.IsAny<TestUnit>(), It.IsAny<MemoryCacheEntryOptions>(),
                        It.IsAny<ICacheStoreOperationMetadata>()), Times.Once);
            distributedStoreMock
                .Verify(
                    x => x.Set("loader:Generic:0", It.IsAny<TestUnit>(), It.IsAny<DistributedCacheEntryOptions>(),
                        It.IsAny<ICacheStoreOperationMetadata>()), Times.Once);
        }
        
        public class OneLayerInheritanceCachingLoader : CachingLoader<int, string>
        {
            protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));
            protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));
            protected override string CacheKey => "OneLayer";
            protected override Task<string?> LoadAsync(int args, CancellationToken token = default) 
                => args.ToString()
                    .Map(Task.FromResult)!;

            protected override string CacheKeySuffixFactory(int args) 
                => args.ToString();
        }
        
        public class GenericCachingLoader<TArgs, TResult> : CachingLoader<TArgs, TResult> where TResult : class, new()
        {
            protected override CachingOptions MemoryCacheOptions => CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));
            protected override CachingOptions DistributedCacheOptions => CachingOptions.Enabled(TimeSpan.FromMilliseconds(1));
            protected override string CacheKey => "Generic";
            protected override Task<TResult?> LoadAsync(TArgs args, CancellationToken token = default) 
                => new TResult().Map(Task.FromResult)!;

            protected override string CacheKeySuffixFactory(TArgs args) 
                => args?.ToString()!;
        }
        
        public class ImplementationGenericLoader : GenericCachingLoader<int, TestUnit>
        {
            protected override Task<TestUnit?> LoadAsync(int args, CancellationToken token = default) 
                => TestUnit.Create().Map(Task.FromResult)!;

            protected override string CacheKeySuffixFactory(int args)
                => args.ToString();
        }

        public class LoaderUsingClass
        {
            public readonly ICachingLoader<int, TestUnit> Loader;

            public LoaderUsingClass(ICachingLoader<int, TestUnit> loader)
            {
                Loader = loader;
            }
        }
    }
}