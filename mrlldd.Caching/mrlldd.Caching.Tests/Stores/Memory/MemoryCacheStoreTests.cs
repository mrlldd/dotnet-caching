using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Stores.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores.Memory
{
    [TestFixture]
    public class MemoryCacheStoreTests : StoreRelatedTest
    {
        [Test]
        public void SuccessIfHit() => Container
            .Effect(c =>
            {
                var memoryCache = c.GetRequiredService<IMemoryCache>();
                var unit = new VoidUnit();
                memoryCache.Set(Key, unit);
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Get<VoidUnit>(Key, NullMetadata.Instance);
                result
                    .Should()
                    .BeSuccessfulResult<VoidUnit>();
                result.UnwrapAsSuccess().Should().Be(unit);
            });

        [Test]
        public Task SuccessIfHitAsync() => Container
            .EffectAsync(async c =>
            {
                var memoryCache = c.GetRequiredService<IMemoryCache>();
                var unit = new VoidUnit();
                memoryCache.Set(Key, unit);
                var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                    .GetAsync<VoidUnit>(Key, DefaultMetadata);
                result
                    .Should()
                    .BeSuccessfulResult<VoidUnit>();
                result.UnwrapAsSuccess().Should().Be(unit);
            });

        [Test]
        public void FailIfMiss() => Container
            .Effect(c =>
            {
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Get<VoidUnit>(Key, DefaultMetadata);
                result
                    .Should()
                    .BeFailResult<VoidUnit>();
                result.UnwrapAsFail()
                    .Should()
                    .NotBeNull()
                    .And.BeOfType<CacheMissException>()
                    .Which.Key.Should().Be(Key);
            });

        [Test]
        public Task FailIfMissAsync() => Container
            .EffectAsync(async c =>
            {
                var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                    .GetAsync<VoidUnit>(Key, DefaultMetadata);
                result
                    .Should()
                    .BeFailResult<VoidUnit>();
                result.UnwrapAsFail()
                    .Should()
                    .NotBeNull()
                    .And.BeOfType<CacheMissException>();
            });

        [Test]
        public void SuccessIfSet() => Container
            .Effect(c =>
            {
                var result = c.GetRequiredService<ICacheStore<InMemory>>()
                    .Set(Key, new VoidUnit(), CachingOptions, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });

        [Test]
        public Task SuccessIfSetAsync() => Container
            .EffectAsync(async c =>
            {
                var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                    .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });

        [Test]
        public void FailIfNotSet() => Container
            .Effect(c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                mock.Setup(x => x.CreateEntry(It.Is<string>(s => s == Key)))
                    .Throws<TestException>();
                var result = c.GetRequiredService<ICacheStore<InMemory>>()
                    .Set(Key, new VoidUnit(), CachingOptions, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });

        [Test]
        public Task FailIfNotSetAsync() => Container
            .EffectAsync(async c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                mock.Setup(x => x.CreateEntry(It.Is<string>(s => s == Key)))
                    .Throws<TestException>();
                var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                    .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });

        [Test]
        public void SuccessIfRefresh() => Container
            .Effect(c =>
            {
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Refresh(Key, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });

        [Test]
        public Task SuccessIfRefreshAsync() => Container
            .EffectAsync(async c =>
            {
                var result = await c.GetRequiredService<ICacheStore<InMemory>>().RefreshAsync(Key, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });

        [Test]
        public void FailIfNotRefresh() => Container
            .Effect(c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                object obj;
                mock.Setup(x => x.TryGetValue(It.Is<string>(s => s == Key), out obj))
                    .Throws<TestException>();
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Refresh(Key, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });

        [Test]
        public Task FailIfNotRefreshAsync() => Container
            .EffectAsync(async c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                object obj;
                mock.Setup(x => x.TryGetValue(It.Is<string>(s => s == Key), out obj))
                    .Throws<TestException>();
                var result = await c.GetRequiredService<ICacheStore<InMemory>>().RefreshAsync(Key, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });

        [Test]
        public void SuccessIfRemove() => Container
            .Effect(c =>
            {
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Remove(Key, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });

        [Test]
        public Task SuccessIfRemoveAsync() => Container
            .EffectAsync(async c =>
            {
                var result = await c.GetRequiredService<ICacheStore<InMemory>>().RemoveAsync(Key, DefaultMetadata);
                result.Should()
                    .BeSuccessfulResult();
            });
        
        [Test]
        public void FailIfNotRemove() => Container
            .Effect(c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                mock.Setup(x => x.Remove(It.Is<string>(s => s == Key)))
                    .Throws<TestException>();
                var result = c.GetRequiredService<ICacheStore<InMemory>>().Remove(Key, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });

        [Test]
        public Task FailIfNotRemoveAsync() => Container
            .EffectAsync(async c =>
            {
                c.AddMock<IMemoryCache>(MockRepository);
                var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                mock.Setup(x => x.Remove(It.Is<string>(s => s == Key)))
                    .Throws<TestException>();
                var result = await c.GetRequiredService<ICacheStore<InMemory>>().RemoveAsync(Key, DefaultMetadata);
                result.Should()
                    .BeFailResult()
                    .Which.Exception.Should().BeOfType<TestException>();
            });
    }
}