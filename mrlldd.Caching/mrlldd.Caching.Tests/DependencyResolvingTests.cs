using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
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
            var caches = sp.GetRequiredService<ICaches<VoidUnit>>();
            caches.Should()
                .NotBeNull()
                .And.BeOfType<Caches<VoidUnit>>();
        }

        [Test]
        public void ResolvesSeparateCaches()
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            var voidCache = sp.GetRequiredService<ICache<VoidUnit, InVoid>>();
            var memoryCache = sp.GetRequiredService<ICache<VoidUnit, InMemory>>();
            voidCache
                .Should()
                .NotBeNull()
                .And.BeOfType<VoidCache>();
            memoryCache
                .Should()
                .NotBeNull()
                .And.BeOfType<MemoryCache>();
        }
    }
}