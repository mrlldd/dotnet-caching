using System;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Tests.TestImplementations.Caches;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    [TestFixture]
    public class CacheProviderTests : CachingTest
    {
        [Test]
        public void FailsIfPassedWrongType()
        {
            Container
                .Effect(c =>
                {
                    var provider = c.GetRequiredService<CachingProvider>();
                    var result = provider.GetRequired(typeof(int));
                    result.Should()
                        .BeFailResult<object>()
                        .Which.Exception.Should()
                        .NotBeNull()
                        .And.BeOfType<ArgumentException>();
                });
        }

        [Test]
        public void SuccessfulIfPassedInheritedType()
        {
            Container
                .Effect(c =>
                {
                    var provider = c.GetRequiredService<CachingProvider>();
                    var result = provider.GetRequired(typeof(IInternalCache<VoidUnit, InMoq>));
                    result.Should()
                        .BeSuccessfulResult<object>()
                        .Which.Value.Should()
                        .NotBeNull()
                        .And.BeOfType<MoqCache>();
                });
        }
    }
}