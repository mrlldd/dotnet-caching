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
        private TimeSpan timeSpan { get; set; } = TimeSpan.Zero;

        [SetUp]
        public void SetUp()
            => timeSpan = new Faker().Random
                .Double()
                .Map(TimeSpan.FromMilliseconds);

        [Test]
        public void EnabledSlidingExpirationIsValid()
        {
            var options = CachingOptions.Enabled(timeSpan);
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeTrue();
            options.SlidingExpiration.Should()
                .Be(timeSpan);
            options.AbsoluteExpirationRelativeToNow.Should()
                .BeNull();
        }

        [Test]
        public void EnabledAbsoluteRelativeToNowIsValid()
        {
            var options = CachingOptions.EnabledAbsoluteRelativeToNow(timeSpan);
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeTrue();
            options.AbsoluteExpirationRelativeToNow
                .Should()
                .Be(timeSpan);
            options.SlidingExpiration
                .Should()
                .BeNull();
        }

        [Test]
        public void EnabledWithSlidingExpirationIsValid()
        {
            var options = CachingOptions.Enabled(timeSpan, timeSpan);
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeTrue();
            options.AbsoluteExpirationRelativeToNow.Should()
                .Be(timeSpan);
            options.SlidingExpiration.Should()
                .Be(timeSpan);
        }
        
        [Test]
        public void EnabledWithoutSlidingExpirationIsValid()
        {
            var options = CachingOptions.Enabled(null, timeSpan);
            options.Should()
                .NotBeNull();
            options.IsCaching.Should()
                .BeTrue();
            options.AbsoluteExpirationRelativeToNow.Should()
                .Be(timeSpan);
            options.SlidingExpiration.Should()
                .BeNull();
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