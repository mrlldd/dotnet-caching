using System;
using FluentAssertions;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestImplementations.Caches;
using mrlldd.Caching.Tests.TestImplementations.Caches.DependencyResolving;
using mrlldd.Caching.Tests.TestImplementations.Flags;
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
        public void ResolvesCachesCollection()
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            var unified = sp.GetRequiredService<ICache<DependencyResolvingUnit>>();
            unified.Should()
                .NotBeNull()
                .And.BeOfType<Cache<DependencyResolvingUnit>>();
            unified.Instances
                .Should()
                .NotBeNull()
                .And.BeOfType<ReadOnlyCachesCollection<DependencyResolvingUnit>>();
            unified.Instances.Count
                .Should()
                .Be(2);

            var vc = sp.GetRequiredService<ICache<DependencyResolvingUnit, InVoid>>();
            var mc = sp.GetRequiredService<ICache<DependencyResolvingUnit, InMemory>>();
            unified.Instances.Should()
                .Contain(vc)
                .And.Contain(mc);
        }


        private static object[] resolvingCases =
        {
            new object[] { typeof(ICache<DependencyResolvingUnit, InVoid>), typeof(DependencyResolvingVoidCache) },
            new object[] { typeof(ICache<DependencyResolvingUnit, InMemory>), typeof(DependencyResolvingMemoryCache) }
        };

        [Test]
        [TestCaseSource(nameof(resolvingCases))]
        public void ResolvesSeparateCaches(Type interfaceType, Type implementationType)
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
            var hasDependency = (HasDependency)service;
            hasDependency.Dependency
                .Should()
                .NotBeNull()
                .And.BeAssignableTo(interfaceType)
                .And.BeOfType(implementationType);
        }

        private class HasDependency
        {
            protected HasDependency(object dependency) 
                => Dependency = dependency;

            public object Dependency {get; }
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
        public void ResolvesGenericImplementation()
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

        [Test]
        public void ThrowsIfThereIsNoAnyStoreProvider()
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            Func<ICache<DependencyResolvingUnit, InNotExistingStore>> func = () => sp.GetRequiredService<ICache<DependencyResolvingUnit, InNotExistingStore>>();
            func.Should().ThrowExactly<StoreNotFoundException<InNotExistingStore>>();
        }
    }
}