using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Functional.Result;
using Functional.Result.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Loaders;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Loaders
{
    public abstract class CacheStoreCallTestFixture : CachingTest
    {
        private const string EntryKey = "loader:moq:args";
        protected abstract CachingOptions Options { get; }

        protected abstract Result Result { get; }
        protected abstract Func<Times> SingleActionHits { get; }

        protected abstract Result<VoidUnit> ResultFactory(VoidUnit args);


        [Test]
        public void LoadsIfStoreMiss() => Container
            .Effect(c => WithExactOperationsCount(() =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, Result<VoidUnit>>> getSetup = x =>
                    x.Get<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                storeMock.Setup(getSetup)
                    .Returns(new CacheMissException(EntryKey))
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, Result>> setSetup = x => x.Set(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>()
                );
                storeMock.Setup(setSetup)
                    .Returns(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = cachingLoader.GetOrLoad(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Times.Once);
                storeMock
                    .Verify(getSetup, SingleActionHits);
                storeMock
                    .Verify(setSetup, SingleActionHits);
            }, Options.IsCaching ? () => Times.Exactly(2) : Times.Never));
        
        [Test]
        public Task LoadsIfStoreMissAsync() => Container
            .EffectAsync(c => WithExactOperationsCountAsync(async () =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result<VoidUnit>>>> getSetup = x =>
                    x.GetAsync<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                storeMock.Setup(getSetup)
                    .ReturnsAsync(new CacheMissException(EntryKey))
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setSetup = x => x.SetAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>()
                );
                storeMock.Setup(setSetup)
                    .ReturnsAsync(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await cachingLoader.GetOrLoadAsync(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Times.Once);
                storeMock
                    .Verify(getSetup, SingleActionHits);
                storeMock
                    .Verify(setSetup, SingleActionHits);
            }, Options.IsCaching ? () => Times.Exactly(2) : Times.Never));

        [Test]
        public void LoadsIfOmittingCache() => Container
            .Effect(c => WithExactOperationsCount(() =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var fromStore = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, Result<VoidUnit>>> getSetup = x =>
                    x.Get<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                storeMock.Setup(getSetup)
                    .Returns(fromStore)
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, Result>> setSetup = x => x.Set(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>()
                );
                storeMock.Setup(setSetup)
                    .Returns(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = cachingLoader.GetOrLoad(unit, true);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Times.Once);
                storeMock
                    .Verify(getSetup, Times.Never);
                storeMock
                    .Verify(setSetup, SingleActionHits);
            }, SingleActionHits));

        [Test]
        public void GetsIfNotOmittingCache() => Container
            .Effect(c => WithExactOperationsCount(() =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var fromStore = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, Result<VoidUnit>>> getSetup = x =>
                    x.Get<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>());
                storeMock.Setup(getSetup)
                    .Returns(fromStore)
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, Result>> setSetup = x => x.Set(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>()
                );
                storeMock.Setup(setSetup)
                    .Returns(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = cachingLoader.GetOrLoad(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Options.IsCaching ? Times.Never : Times.Once);
                storeMock
                    .Verify(getSetup, Options.IsCaching ? Times.Once : Times.Never);
                storeMock
                    .Verify(setSetup, Times.Never);
            }, Options.IsCaching ? Times.Once : Times.Never));

        [Test]
        public Task LoadsIfOmittingCacheAsync() => Container
            .EffectAsync(c => WithExactOperationsCountAsync(async () =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var fromStore = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result<VoidUnit>>>> getSetup = x =>
                    x.GetAsync<VoidUnit>(It.Is<string>(s => s == EntryKey),
                        It.IsAny<ICacheStoreOperationMetadata>(), It.IsAny<CancellationToken>());
                storeMock.Setup(getSetup)
                    .ReturnsAsync(fromStore)
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setSetup = x => x.SetAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>()
                );
                storeMock.Setup(setSetup)
                    .ReturnsAsync(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await cachingLoader.GetOrLoadAsync(unit, true);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Times.Once);
                storeMock
                    .Verify(getSetup, Times.Never);
                storeMock
                    .Verify(setSetup, SingleActionHits);
            }, SingleActionHits));

        [Test]
        public Task GetsIfNotOmittingCacheAsync() => Container
            .EffectAsync(c => WithExactOperationsCountAsync(async () =>
            {
                InjectInstance(Options);
                Container.AddMock<ILoader<VoidUnit, VoidUnit>>(MockRepository);
                var unit = new VoidUnit();
                var loaderMock = c.GetRequiredService<Mock<ILoader<VoidUnit, VoidUnit>>>();
                Expression<Func<ILoader<VoidUnit, VoidUnit>, Task<VoidUnit>>> loadSetup = x =>
                    x.LoadAsync(It.Is<VoidUnit>(u => u == unit), It.IsAny<CancellationToken>());
                loaderMock.Setup(loadSetup)
                    .ReturnsAsync(unit)
                    .Verifiable();
                var storeMock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var fromStore = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result<VoidUnit>>>> getSetup = x =>
                    x.GetAsync<VoidUnit>(It.Is<string>(s => s == EntryKey), It.IsAny<ICacheStoreOperationMetadata>(),
                        It.IsAny<CancellationToken>());
                storeMock.Setup(getSetup)
                    .ReturnsAsync(fromStore)
                    .Verifiable();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setSetup = x => x.SetAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>()
                );
                storeMock.Setup(setSetup)
                    .ReturnsAsync(Result);

                var cachingLoader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await cachingLoader.GetOrLoadAsync(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(unit.AsSuccess());
                loaderMock
                    .Verify(loadSetup, Options.IsCaching ? Times.Never : Times.Once);
                storeMock
                    .Verify(getSetup, Options.IsCaching ? Times.Once : Times.Never);
                storeMock
                    .Verify(setSetup, Times.Never);
            }, Options.IsCaching ? Times.Once : Times.Never));

        [Test]
        public void GetsFromStore() => Container
            .Effect(c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                var expected = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, Result<VoidUnit>>> setup = x => x.Get<VoidUnit>(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup).Returns(expected)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = loader.Get(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(expected);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public Task GetsFromStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                var expected = ResultFactory(unit);
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result<VoidUnit>>>> setup = x => x.GetAsync<VoidUnit>(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>());
                mock.Setup(setup).ReturnsAsync(expected)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await loader.GetAsync(unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(expected);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public void SetsInStore() => Container
            .Effect(c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x => x.Set(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup).Returns(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = loader.Set(unit, unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public Task SetsInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                var unit = new VoidUnit();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x => x.SetAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.Is<VoidUnit>(u => u == unit),
                    It.Is<CachingOptions>(o => o == Options),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>());
                mock.Setup(setup).ReturnsAsync(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await loader.SetAsync(unit, unit);
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public void RefreshesInStore() => Container
            .Effect(c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x => x.Refresh(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup).Returns(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = loader.Refresh(new VoidUnit());
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public Task RefreshesInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x => x.RefreshAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>());
                mock.Setup(setup).ReturnsAsync(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await loader.RefreshAsync(new VoidUnit());
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public void RemovesInStore() => Container
            .Effect(c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, Result>> setup = x => x.Remove(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>());
                mock.Setup(setup).Returns(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = loader.Remove(new VoidUnit());
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });

        [Test]
        public Task RemovesInStoreAsync() => Container
            .EffectAsync(async c =>
            {
                InjectInstance(Options);
                var mock = c.GetRequiredService<Mock<ICacheStore<InMoq>>>();
                Expression<Func<ICacheStore<InMoq>, ValueTask<Result>>> setup = x => x.RemoveAsync(
                    It.Is<string>(s => s == EntryKey),
                    It.IsAny<ICacheStoreOperationMetadata>(),
                    It.IsAny<CancellationToken>());
                mock.Setup(setup).ReturnsAsync(Result)
                    .Verifiable();
                var loader = c.GetRequiredService<ICachingLoader<VoidUnit, VoidUnit, InMoq>>();
                var result = await loader.RemoveAsync(new VoidUnit());
                result.Should()
                    .NotBeNull()
                    .And.BeEquivalentTo(Result);
                mock.Verify(setup, SingleActionHits);
            });
    }
}