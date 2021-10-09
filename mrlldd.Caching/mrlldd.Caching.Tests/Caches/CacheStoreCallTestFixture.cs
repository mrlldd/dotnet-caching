using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestUtilities;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Caches
{
    public abstract class CacheStoreCallTestFixture : CachingTest
    {
        private const string EntryKey = "cache:moq:singleton";
        protected abstract CachingOptions CachingOptions { get; }
        protected abstract Func<Times> Hits { get; }
        
        protected abstract Result Result { get; }

        protected abstract Result<VoidUnit> ResultFactory(VoidUnit unit);

        [TearDown]
        public void TearDown()
        {
            Container.GetRequiredService<Mock<IStoreOperationProvider>>()
                .Verify(OperationProviderSetupExpression, Hits);
        }
        
        [Test]
        public void GetsFromStore() => Container
            .Effect(c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, Result<VoidUnit>>> setup = x =>
                    x.Get<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                var unitResult = ResultFactory(unit);
                mock.Setup(setup)
                    .Returns(unitResult);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = cache.Get();
                result.Should().BeEquivalentTo(unitResult);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public Task GetsFromStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result<VoidUnit>>>> setup = x =>
                    x.GetAsync<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                var unitResult = ResultFactory(unit);
                mock.Setup(setup)
                    .ReturnsAsync(unitResult);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = await cache.GetAsync();
                result.Should().BeEquivalentTo(unitResult);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public void SetsInStore() => Container
            .Effect(c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x =>
                    x.Set(It.Is<string>(s => s == EntryKey), It.Is<VoidUnit>(u => u == unit),
                        It.Is<CachingOptions>(o => o == CachingOptions), It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup)
                    .Returns(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = cache.Set(unit);
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public Task SetsInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x =>
                    x.SetAsync(It.Is<string>(s => s == EntryKey), It.Is<VoidUnit>(u => u == unit),
                        It.Is<CachingOptions>(o => o == CachingOptions), It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                mock.Setup(setup)
                    .ReturnsAsync(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = await cache.SetAsync(unit);
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public void RefreshesInStore() => Container
            .Effect(c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x =>
                    x.Refresh(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup)
                    .Returns(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = cache.Refresh();
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public Task RefreshesInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x =>
                    x.RefreshAsync(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                mock.Setup(setup)
                    .ReturnsAsync(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = await cache.RefreshAsync();
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public void RemovesInStore() => Container
            .Effect(c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x =>
                    x.Remove(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup)
                    .Returns(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = cache.Remove();
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
        
        [Test]
        public Task RemovesInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(CachingOptions);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x =>
                    x.RemoveAsync(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                mock.Setup(setup)
                    .ReturnsAsync(Result);
                var cache = c.GetRequiredService<ICache<VoidUnit, InMoq>>();
                var result = await cache.RemoveAsync();
                result.Should().BeEquivalentTo(Result);
                mock.Verify(setup, Hits);
            });
    }
}