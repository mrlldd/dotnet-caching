using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.InvalidAssembly;
using mrlldd.Caching.Tests.InvalidAssembly.Flags;
using mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving;
using mrlldd.Caching.Tests.TestImplementations.Flags;
using mrlldd.Caching.Tests.TestImplementations.Loaders.DependencyResolving;
using mrlldd.Caching.Tests.TestImplementations.Stores;
using mrlldd.Caching.Tests.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    [TestFixture]
    public class DependencyResolvingTests
    {
        [Test]
        public void Builds()
        {
            Func<IServiceProvider> func = () => new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            func.Should().NotThrow();
        }

        [Test]
        public void ThrowsIfNoAssemblies()
        {
            var services = new ServiceCollection();
            Func<ICachingServiceCollection> func = () => services.AddCaching();
            func.Should().ThrowExactly<InvalidOperationException>();
        }

        [Test]
        public void ResolvesCachesCollection()
        {
            var assembly = typeof(DependencyResolvingTests).Assembly;
            var sp = new ServiceCollection()
                .AddCaching(assembly)
                .BuildServiceProvider();
            var unified = sp.GetRequiredService<ICache<DependencyResolvingUnit>>();
            unified.Should()
                .NotBeNull()
                .And.BeOfType<Cache<DependencyResolvingUnit>>();
            unified.Instances
                .Should()
                .NotBeNull()
                .And.BeOfType<ReadOnlyCachesCollection<DependencyResolvingUnit>>();
            var count = assembly
                .GetTypes()
                .Count(t => !string.IsNullOrEmpty(t.Namespace) &&
                            !t.IsAbstract &&
                            !t.IsGenericTypeDefinition &&
                            t.Namespace.EndsWith("TestImplementations.Caches.DependencyResolving"));
            unified.Instances.Count
                .Should()
                .Be(count);

            var vc = sp.GetRequiredService<ICache<DependencyResolvingUnit, InVoid>>();
            var mc = sp.GetRequiredService<ICache<DependencyResolvingUnit, InMemory>>();
            unified.Instances.Should()
                .Contain(vc)
                .And.Contain(mc);
        }

        [Test]
        public void ResolvesLoader()
        {
            var assembly = typeof(DependencyResolvingTests).Assembly;
            var sp = new ServiceCollection()
                .AddCaching(assembly)
                .BuildServiceProvider();
            var loader = sp.GetRequiredService<ILoader<DependencyResolvingUnit, string>>();
            loader.Should()
                .NotBeNull()
                .And.BeOfType<DependencyResolvingLoader>();
        }


        private static object[] resolvingCases =
        {
            new object[] {typeof(ICache<DependencyResolvingUnit, InVoid>), typeof(DependencyResolvingVoidCache)},
            new object[] {typeof(ICache<DependencyResolvingUnit, InMemory>), typeof(DependencyResolvingMemoryCache)},
            new object[] {typeof(ILoader<DependencyResolvingUnit, string>), typeof(DependencyResolvingLoader)},
            new object[]
            {
                typeof(ICachingLoader<DependencyResolvingUnit, string, InVoid>),
                typeof(DependencyResolvingVoidCachingLoader)
            }
        };

        [Test]
        [TestCaseSource(nameof(resolvingCases))]
        public void ResolvesSeparateServices(Type interfaceType, Type implementationType)
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            var service = sp.GetRequiredService(interfaceType);
            service
                .Should()
                .NotBeNull()
                .And.BeOfType(implementationType);
        }


        private static object[] usingServicesCases =
        {
            new object[]
            {
                typeof(CacheUsingClass),
                typeof(ICache<DependencyResolvingUnit, InVoid>),
                typeof(DependencyResolvingVoidCache)
            },
            new object[]
            {
                typeof(CacheCollectionUsingClass),
                typeof(ICache<DependencyResolvingUnit>),
                typeof(Cache<DependencyResolvingUnit>)
            },
            new object[]
            {
                typeof(LoaderUsingClass),
                typeof(ILoader<DependencyResolvingUnit, string>),
                typeof(DependencyResolvingLoader)
            },
            new object[]
            {
                typeof(CachingLoaderUsingClass),
                typeof(ICachingLoader<DependencyResolvingUnit, string, InVoid>),
                typeof(DependencyResolvingVoidCachingLoader)
            }
        };


        [Test]
        [TestCaseSource(nameof(usingServicesCases))]
        public void ResolvesUsingService(Type usingServiceType, Type interfaceType, Type implementationType)
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .AddScoped(usingServiceType)
                .BuildServiceProvider();
            var service = sp.GetRequiredService(usingServiceType);
            service
                .Should()
                .NotBeNull()
                .And.BeOfType(usingServiceType)
                .And.BeAssignableTo<HasDependency>();
            var hasDependency = (HasDependency) service;
            hasDependency.Dependency
                .Should()
                .NotBeNull()
                .And.BeAssignableTo(interfaceType)
                .And.BeOfType(implementationType);
        }

        private class HasDependency
        {
            protected HasDependency(object dependency)
            {
                Dependency = dependency;
            }

            public object Dependency { get; }
        }

        private class LoaderUsingClass : HasDependency
        {
            public LoaderUsingClass(ILoader<DependencyResolvingUnit, string> dependency) : base(dependency)
            {
            }
        }

        private class CachingLoaderUsingClass : HasDependency
        {
            public CachingLoaderUsingClass(ICachingLoader<DependencyResolvingUnit, string, InVoid> dependency) :
                base(dependency)
            {
            }
        }

        private class CacheUsingClass : HasDependency
        {
            public CacheUsingClass(ICache<DependencyResolvingUnit, InVoid> dependency) : base(dependency)
            {
            }
        }

        private class CacheCollectionUsingClass : HasDependency
        {
            public CacheCollectionUsingClass(ICache<DependencyResolvingUnit> dependency) : base(dependency)
            {
            }
        }

        [Test]
        public void ResolvesCacheGenericImplementation()
        {
            var sp = new ServiceCollection()
                .UseCachingStore<InGenericScope, GenericScopeStore>()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            var service = sp.GetRequiredService<ICache<DependencyResolvingUnit, InGenericScope>>();
            service
                .Should()
                .NotBeNull()
                .And.BeOfType<GenericImplDependencyResolvingCache>();
        }

        private static object[] notExistingStoreCases =
        {
            typeof(ICache<InvalidUnit, InNotExistingStore>),
            typeof(ICachingLoader<InvalidUnit, InvalidUnit, InNotExistingStore>)
        };

        [Test]
        [TestCaseSource(nameof(notExistingStoreCases))]
        public void ThrowsIfStoreNotFound(Type serviceType)
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(InvalidUnit).Assembly)
                .BuildServiceProvider();
            Func<object> func = () =>
                sp.GetRequiredService(serviceType);
            func.Should().ThrowExactly<StoreNotFoundException<InNotExistingStore>>();
        }
    }
}