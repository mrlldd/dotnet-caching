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
        public void SuccessIfHit()
        {
            Container
                .Effect(c =>
                {
                    var memoryCache = c.GetRequiredService<IMemoryCache>();
                    var unit = new VoidUnit();
                    memoryCache.Set(Key, unit);
                    var result = c.GetRequiredService<ICacheStore<InMemory>>()
                        .Get<VoidUnit>(Key, NullOperationOptions.Instance);
                    result
                        .Should()
                        .BeSuccessfulResult<VoidUnit>();
                    result.UnwrapAsSuccess().Should().Be(unit);
                });
        }

        [Test]
        public Task SuccessIfHitAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var memoryCache = c.GetRequiredService<IMemoryCache>();
                    var unit = new VoidUnit();
                    memoryCache.Set(Key, unit);
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                        .GetAsync<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult<VoidUnit>();
                    result.UnwrapAsSuccess().Should().Be(unit);
                });
        }

        [Test]
        public void FailIfMiss()
        {
            Container
                .Effect(c =>
                {
                    var result = c.GetRequiredService<ICacheStore<InMemory>>().Get<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeFailResult<VoidUnit>();
                    result.UnwrapAsFail()
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<CacheMissException>()
                        .Which.Key.Should().Be(Key);
                });
        }

        [Test]
        public Task FailIfMissAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                        .GetAsync<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeFailResult<VoidUnit>();
                    result.UnwrapAsFail()
                        .Should()
                        .NotBeNull()
                        .And.BeOfType<CacheMissException>();
                });
        }

        [Test]
        public void SuccessIfSet()
        {
            Container
                .Effect(c =>
                {
                    var result = c.GetRequiredService<ICacheStore<InMemory>>()
                        .Set(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessIfSetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void FailIfNotSet()
        {
            Container
                .Effect(c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    mock.Setup(x => x.CreateEntry(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InMemory>>()
                        .Set(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }

        [Test]
        public Task FailIfNotSetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    mock.Setup(x => x.CreateEntry(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }

        [Test]
        public void SuccessIfRefresh()
        {
            Container
                .Effect(c =>
                {
                    var result = c.GetRequiredService<ICacheStore<InMemory>>().Refresh(Key, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessIfRefreshAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>().RefreshAsync(Key, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void FailIfNotRefresh()
        {
            Container
                .Effect(c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    object obj;
                    mock.Setup(x => x.TryGetValue(It.Is<string>(s => s == Key), out obj))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InMemory>>().Refresh(Key, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }

        [Test]
        public Task FailIfNotRefreshAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    object obj;
                    mock.Setup(x => x.TryGetValue(It.Is<string>(s => s == Key), out obj))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>().RefreshAsync(Key, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }

        [Test]
        public void SuccessIfRemove()
        {
            Container
                .Effect(c =>
                {
                    var result = c.GetRequiredService<ICacheStore<InMemory>>().Remove(Key, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessIfRemoveAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>().RemoveAsync(Key, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void FailIfNotRemove()
        {
            Container
                .Effect(c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    mock.Setup(x => x.Remove(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InMemory>>().Remove(Key, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }

        [Test]
        public Task FailIfNotRemoveAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    c.AddMock<IMemoryCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IMemoryCache>>();
                    mock.Setup(x => x.Remove(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InMemory>>().RemoveAsync(Key, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }
    }
}