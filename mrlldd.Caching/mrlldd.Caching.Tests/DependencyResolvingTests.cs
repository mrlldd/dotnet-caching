using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Tests.TestImplementations.Caches;
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
    }
}