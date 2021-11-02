using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.Stores.Base;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores.Distributed
{
    [TestFixture]
    public class DistributedCacheStoreTests : StoreRelatedTest
    {
        protected override void FillServicesCollection(IServiceCollection services)
        {
            base.FillServicesCollection(services);
            services.AddDistributedMemoryCache();
        }

        [Test]
        public void SuccessIfHit()
        {
            Container
                .Effect(c =>
                {
                    var distributedCache = c.GetRequiredService<IDistributedCache>();
                    var unit = new VoidUnit();
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unit));
                    distributedCache.Set(Key, bytes);
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>().Get<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult<VoidUnit>();
                    result.UnwrapAsSuccess().Should().BeEquivalentTo(unit);
                });
        }

        [Test]
        public Task SuccessIfHitAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var distributedCache = c.GetRequiredService<IDistributedCache>();
                    var unit = new VoidUnit();
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(unit));
                    await distributedCache.SetAsync(Key, bytes);
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .GetAsync<VoidUnit>(Key, DefaultOperationOptions);
                    result
                        .Should()
                        .BeSuccessfulResult<VoidUnit>();
                    result.UnwrapAsSuccess().Should().BeEquivalentTo(unit);
                });
        }

        [Test]
        public void FailIfMiss()
        {
            Container
                .Effect(c =>
                {
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>().Get<VoidUnit>(Key, DefaultOperationOptions);
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
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
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
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>()
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
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .SetAsync(Key, new VoidUnit(), CachingOptions, DefaultOperationOptions);
                    result.Should()
                        .BeSuccessfulResult();
                });
        }

        [Test]
        public void FailIfNotSet()
        {
            Container
                .Effect(async c =>
                {
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    var serializationResult = await DefaultOperationOptions.Serializer.SerializeAsync(new VoidUnit());
                    var bytes = serializationResult.UnwrapAsSuccess();
                    mock.Setup(x => x.Set(It.Is<string>(s => s == Key), It.Is<byte[]>(b => b.SequenceEqual(bytes)),
                            It.Is<DistributedCacheEntryOptions>(
                                o => o.SlidingExpiration == CachingOptions.SlidingExpiration)))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>()
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
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    var unit = new VoidUnit();
                    var serializationResult = await DefaultOperationOptions.Serializer.SerializeAsync(new VoidUnit());
                    var bytes = serializationResult.UnwrapAsSuccess();
                    mock.Setup(x => x.SetAsync(It.Is<string>(s => s == Key),
                            It.Is<byte[]>(b => b.SequenceEqual(bytes)),
                            It.Is<DistributedCacheEntryOptions>(o =>
                                o.SlidingExpiration == CachingOptions.SlidingExpiration),
                            It.IsAny<CancellationToken>()))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .SetAsync(Key, unit, CachingOptions, DefaultOperationOptions);
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
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RefreshAsync(Key, DefaultOperationOptions);
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
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    mock.Setup(x => x.Refresh(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>().Refresh(Key, DefaultOperationOptions);
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
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    mock.Setup(x => x.RefreshAsync(It.Is<string>(s => s == Key), It.IsAny<CancellationToken>()))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RefreshAsync(Key, DefaultOperationOptions);
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
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>().Remove(Key, DefaultOperationOptions);
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
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RemoveAsync(Key, DefaultOperationOptions);
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
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    mock.Setup(x => x.Remove(It.Is<string>(s => s == Key)))
                        .Throws<TestException>();
                    var result = c.GetRequiredService<ICacheStore<InDistributed>>().Remove(Key, DefaultOperationOptions);
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
                    c.AddMock<IDistributedCache>(MockRepository);
                    var mock = c.GetRequiredService<Mock<IDistributedCache>>();
                    mock.Setup(x => x.RemoveAsync(It.Is<string>(s => s == Key), It.IsAny<CancellationToken>()))
                        .Throws<TestException>();
                    var result = await c.GetRequiredService<ICacheStore<InDistributed>>()
                        .RemoveAsync(Key, DefaultOperationOptions);
                    result.Should()
                        .BeFailResult()
                        .Which.Exception.Should().BeOfType<TestException>();
                });
        }
    }
}