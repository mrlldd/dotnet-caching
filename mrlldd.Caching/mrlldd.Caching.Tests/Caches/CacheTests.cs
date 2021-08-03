using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using DryIoc;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Tests.Caches.TestUtilities;
using mrlldd.Caching.Tests.Caches.TestUtilities.Extensions;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    public class CacheTests : CacheRelatedTest
    {
        private readonly string cacheKey = $"test:testcache:{nameof(TestUnit)}";
        [Test]
        public Task CachesToMemory() => Container
            .Map(async c =>
            {
                var mock = MockRepository
                    .Create<IMemoryCache>()
                    .Effect(x =>
                        x.Setup(mc => mc.CreateEntry(It.Is<string>(s => s ==cacheKey)))
                            .Returns(MockRepository.Create<ICacheEntry>().Object)
                            .Verifiable())
                    .AddToContainer(c);

                var provider = c.Resolve<ICacheProvider>();

                var cache = provider.Get<TestUnit>();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                mock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                        Times.Once);
            });
        
        [Test]
        public Task NotCachingToMemoryIfDisabled() => Container
            .WithDistributedCacheOnly()
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

                var cache = provider.Get<TestUnit>();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                mock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)),
                        Times.Never);
            });

        [Test]
        public Task NotTryingToGetFromMemoryIfDisabled() => Container
            .WithDistributedCacheOnly()
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

                var cache = provider.Get<TestUnit>();

                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                await cache.GetAsync();
                mock.Effect(x =>
                        x.Verify(mc => mc.CreateEntry(It.Is<string>(s => s ==cacheKey)),
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
                var cache = provider.Get<TestUnit>();

                await cache.SetAsync(unit);
                mock.Verify(x => x.SetAsync(cacheKey,
                    It.Is<byte[]>(b => Encoding.UTF8.GetBytes(serialized).SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()), Times.Once);
            });

        [Test]
        public Task NotCachingToDistributedIfDisabled() => Container
            .WithMemoryCacheOnly()
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
                var cache = provider.Get<TestUnit>();

                await cache.SetAsync(unit);
                mock.Verify(x => x.SetAsync(cacheKey,
                    It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(),
                    It.IsAny<CancellationToken>()), Times.Never);
            });

        [Test]
        public Task GetsUntouchedFromMemory() => Container
            .WithMemoryCacheOnly()
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.Get<TestUnit>();
                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                var fromCache = await cache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
            });

        [Test]
        public Task GetsUntouchedFromDistributed() => Container
            .WithDistributedCacheOnly()
            .WithFakeDistributedCache()
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.Get<TestUnit>();
                var unit = TestUnit.Create();
                await cache.SetAsync(unit);
                var fromCache = await cache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
            });

        [Test]
        public Task GetUntouchedFromMemory() => Container
            .WithMemoryCacheOnly()
            .Map(async c =>
            {
                var provider = c.Resolve<ICacheProvider>();
                var cache = provider.Get<TestUnit>();
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
                            .Returns<string, byte[], DistributedCacheEntryOptions, CancellationToken>((s, b, o, ct) => distributedCache.SetAsync(s, b, o, ct))
                            .Verifiable())
                    .Effect(x => 
                        x.Setup(dc => dc.GetAsync(It.Is<string>(s => s == cacheKey), It.IsAny<CancellationToken>()))
                            .Returns<string, CancellationToken>((s, ct) => distributedCache.GetAsync(s, ct)))
                    .AddToContainer(c);
                var provider = c.Resolve<ICacheProvider>();
                var ourCache = provider.Get<TestUnit>();
                await ourCache.SetAsync(unit);
                mcMock.Object.Remove(cacheKey);
                var fromCache = await ourCache.GetAsync();
                fromCache.Should().BeEquivalentTo(unit);
                mcMock.Verify(x => x.CreateEntry(It.Is<string>(s => s == cacheKey)), Times.Exactly(2));
                mcMock.Verify(x => x.Remove(It.Is<string>(s => s == cacheKey)), Times.Once);
                mcMock.Verify(x => x.TryGetValue(It.Is<string>(s => s == cacheKey), out It.Ref<object>.IsAny), Times.Once);
                dcMock.Verify(x => x.SetAsync(It.Is<string>(s => s == cacheKey),
                    It.Is<byte[]>(b => serialized.SequenceEqual(b)),
                    It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
                
            });
    }
}