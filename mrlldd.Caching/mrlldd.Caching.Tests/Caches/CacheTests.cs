using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Caches.TestUtilities;
using mrlldd.Caching.Tests.Caches.TestUtilities.Extensions;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    [TestFixture]
    public class CacheTests : CacheRelatedTest
    {
        private readonly string cacheKey =
            $"{TestCache<TestUnit>.GlobalCacheKey}:{nameof(TestUnit)}";

        [Test]
        public Task UsesMemoryBubbleIfMemoryDisabled() => Container
            .WithDistributedCacheOnly<TestUnit>()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var unit = TestUnit.Create();
                await provider.GetRequired<TestUnit>().UnwrapAsSuccess().SetAsync(unit);
                c.Resolve<Mock<IBubbleCacheStore>>()
                    .Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey), It.Is<TestUnit>(u => u == unit),
                        It.IsAny<MemoryCacheEntryOptions>(), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            });
        
        [Test]
        public Task UsesDistributedBubbleIfDistributedDisabled() => Container
            .WithMemoryCacheOnly<TestUnit>()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var unit = TestUnit.Create();
                await provider.GetRequired<TestUnit>().UnwrapAsSuccess().SetAsync(unit);
                c.Resolve<Mock<IBubbleCacheStore>>()
                    .Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey), It.Is<TestUnit>(u => u == unit),
                        It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            });

        [Test]
        public Task CachesToMemory() => Container
            .Map(async c =>
            {
                var mock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)))
                            .Returns(MockRepository.Create<ICacheEntry>().Object)
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();

                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                mock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                    Times.Once);
            });

        [Test]
        public Task NotCachingToMemoryIfDisabled() => Container
            .WithDistributedCacheOnly<TestUnit>()
            .Map(async c =>
            {
                var mock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)))
                            .Returns(MockRepository.Create<ICacheEntry>().Object)
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();

                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                mock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                    Times.Never);
            });

        [Test]
        public Task NotTryingToGetFromMemoryIfDisabled() => Container
            .WithDistributedCacheOnly<TestUnit>()
            .WithFakeDistributedCache()
            .Map(async c =>
            {
                var mock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)))
                            .Returns(MockRepository.Create<ICacheEntry>().Object)
                            .Verifiable())
                    .Effect(x =>
                        x.Setup(mc =>
                                mc.TryGetValue(It.Is<string>(s => s == cacheKey),
                                    out It.Ref<object>.IsAny))
                            .Verifiable()
                    )
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();

                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                await cache.GetAsync();
                mock.Effect(x =>
                        x.Verify(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)),
                            Times.Never))
                    .Verify(
                        mc => mc.TryGetValue(It.Is<string>(s => s == cacheKey),
                            out It.Ref<object>.IsAny), Times.Never);
            });

        [Test]
        public Task CachesToDistributed() => Container
            .Map(async c =>
            {
                var unit = TestUnit.Create();
                var serialized = JsonSerializer.Serialize(unit);
                var mock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x =>
                        x.Setup(dc => dc.SetAsync(cacheKey,
                                It.Is<byte[]>(b => Encoding.UTF8.GetBytes(serialized).SequenceEqual(b)),
                                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();

                await cache.SetAsync(unit);
                mock.Verify(x => x.SetAsync(cacheKey,
                    It.Is<byte[]>(b => Encoding.UTF8.GetBytes(serialized).SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            });

        [Test]
        public Task NotCachingToDistributedIfDisabled() => Container
            .WithMemoryCacheOnly<TestUnit>()
            .Map(async c =>
            {
                var unit = TestUnit.Create();
                var serialized = JsonSerializer.SerializeToUtf8Bytes(unit);
                var mock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x =>
                        x.Setup(dc => dc.SetAsync(cacheKey,
                                It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();

                await cache.SetAsync(unit);
                mock.Verify(x => x.SetAsync(cacheKey,
                    It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()), Times.Never);
            });

        [Test]
        public Task GetsUntouchedFromMemory()
            => Container
                .WithMemoryCacheOnly<TestUnit>()
                .MockStores(MockRepository)
                .Map(async c =>
                {
                    var provider = c.Resolve<ICacheProvider>();
                    var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                    var unit = TestUnit.Create();
                    await cache.SetAsync(unit);
                    var fromCache = await cache.GetAsync();
                    fromCache.Should().BeEquivalentTo(unit);
                });

        [Test]
        public Task GetsUntouchedFromDistributed() => Container
            .WithDistributedCacheOnly<TestUnit>()
            .WithFakeDistributedCache()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                var fromCache = await cache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
            });

        [Test]
        public Task GetsFromDistributedIfMissingInMemory() => Container
            .WithFakeDistributedCache()
            .Map(async c =>
            {
                var memoryCache = c.Resolve<IMemoryCache>();
                var distributedCache = c.Resolve<IDistributedCache>();
                var unit = TestUnit.Create();

                var mcMock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)))
                            .Returns<string>(k => memoryCache.CreateEntry(k))
                            .Verifiable())
                    .Effect(x =>
                        x.Setup(mc => mc.Remove(It.Is<string>(s => s == cacheKey)))
                            .Callback<object>(k => memoryCache.Remove(k))
                            .Verifiable())
                    .AddToContainer(c);
                var serialized = JsonSerializer.SerializeToUtf8Bytes(unit);
                var dcMock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x =>
                        x.Setup(dc => dc.SetAsync(It.Is<string>(s => s == cacheKey),
                                It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                            .Returns<string, byte[], DistributedCacheEntryOptions, CancellationToken>((s, b, o, ct) =>
                                distributedCache.SetAsync(s, b, o, ct))
                            .Verifiable())
                    .Effect(x =>
                        x.Setup(dc => dc.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()))
                            .Returns<string, CancellationToken>((s, ct) => distributedCache.GetAsync(s, ct)))
                    .AddToContainer(c);
                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                await ourCache.SetAsync(unit);
                mcMock.Object.Remove(cacheKey);
                var fromCache = await ourCache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
                mcMock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                    Times.Exactly(2)); // 2 as it saves to memory if successfully got from distributed
                mcMock.Verify(x => x.Remove(It.Is<string>(s => s == cacheKey)), Times.Once);
                mcMock.Verify(x => x.TryGetValue(It.Is<string>(s => s == cacheKey), out It.Ref<object>.IsAny),
                    Times.Once);
                dcMock.Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey),
                    It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
                dcMock.Verify(x => x.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()),
                    Times.Once);
            });

        [Test]
        public Task MemoryCacheHasMorePriority() => Container
            .WithFakeDistributedCache()
            .Map(async c =>
            {
                var memoryCache = c.Resolve<IMemoryCache>();
                var distributedCache = c.Resolve<IDistributedCache>();
                var unit = TestUnit.Create();
                var serialized = JsonSerializer.SerializeToUtf8Bytes(unit);
                object asObject = serialized;
                var mcMock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s == cacheKey)))
                            .Returns<string>(k => memoryCache.CreateEntry(k))
                            .Verifiable())
                    .Effect(x =>
                        x.Setup(mc => mc.TryGetValue(It.Is<string>(s => s == cacheKey), out asObject))
                            .Returns(true)
                            .Verifiable())
                    .AddToContainer(c);
                var dcMock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x =>
                        x.Setup(dc => dc.SetAsync(It.Is<string>(s => s == cacheKey),
                                It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
                            .Returns<string, byte[], DistributedCacheEntryOptions, CancellationToken>((s, b, o, ct) =>
                                distributedCache.SetAsync(s, b, o, ct))
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                await ourCache.SetAsync(unit);
                var fromCache = await ourCache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
                mcMock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                    Times.Exactly(1));
                mcMock.Verify(x => x.TryGetValue(It.Is<string>(s => s == cacheKey), out asObject),
                    Times.Once);
                dcMock.Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey),
                    It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
                dcMock.Verify(x => x.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()),
                    Times.Never);
            });

        [Test]
        public Task DoNothingWhenBothCachesDisabled() => Container
            .WithNoCaches<TestUnit>()
            .Map(async c =>
            {
                var unit = TestUnit.Create();
                var mcMock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x => x.Setup(mc => mc.CreateEntry(It.IsAny<string>())).Verifiable())
                    .Effect(x =>
                        x.Setup(mc => mc.TryGetValue(It.IsAny<string>(), out It.Ref<object>.IsAny)).Verifiable())
                    .AddToContainer(c);

                var dcMock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x => x.Setup(mc => mc.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                        It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>())).Verifiable())
                    .Effect(x =>
                        x.Setup(mc => mc.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                await cache.SetAsync(unit);
                var result = await cache.GetAsync();
                result.Should().Be(default(TestUnit));
                mcMock.Verify(mc => mc.CreateEntry(It.IsAny<string>()), Times.Never);
                mcMock.Verify(mc => mc.TryGetValue(It.IsAny<string>(), out It.Ref<object>.IsAny), Times.Never);
                dcMock.Verify(dc => dc.SetAsync(It.IsAny<string>(),
                    It.IsAny<byte[]>(),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()), Times.Never);
                dcMock.Verify(dc => dc.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            });

        [Test]
        public Task ReturnsDefaultIfCacheEntriesRemovedOrExpired() => Container
            .WithFakeDistributedCache()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var dsMock = c.Resolve<Mock<IDistributedCacheStore>>();
                var msMock = c.Resolve<Mock<IMemoryCacheStore>>();
                var unit = TestUnit.Create();
                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                await ourCache.SetAsync(unit);
                await msMock.Object.RemoveAsync(cacheKey, NullCacheStoreOperationMetadata.Instance);
                await dsMock.Object.RemoveAsync(cacheKey, NullCacheStoreOperationMetadata.Instance);
                var fromCache = await ourCache.GetAsync();
                fromCache.Should().Be(default(TestUnit));
                msMock.Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey),
                        It.Is<TestUnit>(u => unit.PublicProperty == u.PublicProperty),
                        It.IsAny<MemoryCacheEntryOptions>(),
                        It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()),
                    Times.Exactly(1)); // 2 as it saves to memory if successfully got from distributed
                msMock.Verify(
                    x => x.RemoveAsync(It.Is<string>(s => s == cacheKey), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()), Times.Once);
                msMock.Verify(
                    x => x.GetAsync<TestUnit>(It.Is<string>(s => s == cacheKey),
                        It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>()),
                    Times.Once);
                dsMock.Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey),
                    It.Is<TestUnit>(u => unit.PublicProperty == u.PublicProperty),
                    It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>()), Times.Once);
                dsMock.Verify(
                    x => x.RemoveAsync(It.Is<string>(s => s == cacheKey), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
                dsMock.Verify(
                    x => x.GetAsync<TestUnit>(It.Is<string>(s => s == cacheKey),
                        It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            });

        [Test]
        public Task ReturnsDefaultIfDistributedCacheEntryIsInvalid() => Container
            .WithDistributedCacheOnly<TestUnit>()
            .WithFakeDistributedCache()
            .Map(async c =>
            {
                var distributedCache = c.Resolve<IDistributedCache>();
                var unit = TestUnit.Create();
                var dcMock = MockRepository
                    .Create<IDistributedCache>()
                    .Effect(x =>
                        x.Setup(dc => dc.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()))
                            .Returns<string, CancellationToken>((s, ct) => distributedCache.GetAsync(s, ct)))
                    .AddToContainer(c);
                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                var serialized = JsonSerializer.SerializeToUtf8Bytes(unit);
                await distributedCache.SetAsync(cacheKey, serialized.Take(3).ToArray());
                var fromCache = await ourCache.GetAsync();
                fromCache.Should().Be(default(TestUnit));
                dcMock.Verify(x => x.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()),
                    Times.Once);
            });

        [Test]
        public Task AlsoCleansMemoryCacheIfDistributedCacheEntryIsInvalid() => Container
            .WithFakeDistributedCache()
            .MockStores(MockRepository)
            .Map(async c =>
            {
                var distributedStore = c.Resolve<Mock<IDistributedCacheStore>>();
                var memoryStore = c.Resolve<Mock<IMemoryCacheStore>>();
                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.GetRequired<TestUnit>().UnwrapAsSuccess();
                var serialized = TestUnit.Create().Map(x => JsonSerializer.SerializeToUtf8Bytes(x));
                await distributedStore.Object.SetAsync(cacheKey, serialized.Take(3).ToArray(),
                    new DistributedCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.MaxValue
                    }, NullCacheStoreOperationMetadata.Instance);

                var fromCache = await ourCache.GetAsync();
                fromCache.Should().Be(default(TestUnit));
                distributedStore.Verify(
                    x => x.GetAsync<TestUnit>(It.Is<string>(s => s == cacheKey),
                        It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>()),
                    Times.Once);
                memoryStore.Verify(
                    x => x.RemoveAsync(It.Is<string>(s => s == cacheKey), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>()), Times.Once);
            });
    }
}