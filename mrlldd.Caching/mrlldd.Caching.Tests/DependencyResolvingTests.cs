using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Extensions.DependencyInjection;
using mrlldd.Caching.Flags;
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

        public void BuildsRight()
        {
            var sp = new ServiceCollection()
                .AddCaching(typeof(DependencyResolvingTests).Assembly)
                .BuildServiceProvider();
            var caches = sp.GetRequiredService<IEnumerable<ICache<VoidUnit>>>()
                .ToArray();
            caches.Should()
                .NotBeNull()
                .And.HaveCount(2)
                .And.OnlyContain(x => x != null);
            caches.Should()
                .ContainSingle(x => x is Cache<VoidUnit, InVoid>);
            caches.Should()
                .ContainSingle(x => x is Cache<VoidUnit, InMemory>);
        }
    }
}