using System;
using Bogus;
using FluentAssertions;
using Functional.Object.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    [TestFixture]
    public class CachingOptionsTests
    {
        [Test]
        public void EnabledIsValid()
        {
            var timeSpan = new Faker().Random
                .Double()
                .Map(TimeSpan.FromMilliseconds);
            var options = CachingOptions.Enabled(timeSpan);
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeTrue();
            options.SlidingExpiration.Should()
                .Be(timeSpan);
        }

        [Test]
        public void DisabledIsValid()
        {
            var options = CachingOptions.Disabled;
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeFalse();
            options.SlidingExpiration.Should()
                .BeNull();
        }
    }
}