﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using mrlldd.Caching.Exceptions;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Serializers;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Internal;
using mrlldd.Caching.Tests.TestUtilities;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores
{
    [TestFixture]
    public class NoOpDistributedCacheStoreTests : TestBase
    {
        private string Key { get; set; } = null!;
        private ICacheStoreOperationOptions OperationOptions { get; set; } = null!;

        private CachingOptions CachingOptions { get; set; } = null!;

        protected override void AfterContainerEnriching()
        {
            base.AfterContainerEnriching();
            Key = Faker.Random.String(0, 32);
            OperationOptions =
                new CacheStoreOperationOptions(Faker.Random.Number(0, 99999), Faker.Random.String(0, 32), new NewtonsoftJsonCachingSerializer());
            CachingOptions = CachingOptions.Enabled(TimeSpan.FromMilliseconds(Faker.Random.Double(0, 99999)));
        }

        protected override void FillServicesCollection(IServiceCollection services)
        {
            base.FillServicesCollection(services);
            services.AddScoped<IDistributedCache, NoOpDistributedCache>();
        }

        [Test]
        public void FailOnGet()
        {
            Container
                .Effect(c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = store.Get<VoidUnit>(Key, OperationOptions);
                    result.Should()
                        .BeFailResult<VoidUnit>()
                        .Which.Exception.Should()
                        .NotBeNull()
                        .And.BeOfType<CacheMissException>();
                });
        }

        [Test]
        public Task FailOnGetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = await store.GetAsync<VoidUnit>(Key, OperationOptions);
                    result.Should()
                        .BeFailResult<VoidUnit>()
                        .Which.Exception.Should()
                        .NotBeNull()
                        .And.BeOfType<CacheMissException>();
                });
        }

        [Test]
        public void SuccessOnSet()
        {
            Container
                .Effect(c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = store.Set(Key, new VoidUnit(), CachingOptions, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessOnSetAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = await store.SetAsync(Key, new VoidUnit(), CachingOptions, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }

        [Test]
        public void SuccessOnRefresh()
        {
            Container
                .Effect(c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = store.Refresh(Key, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessOnRefreshAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = await store.RefreshAsync(Key, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }

        [Test]
        public void SuccessOnRemove()
        {
            Container
                .Effect(c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = store.Remove(Key, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }

        [Test]
        public Task SuccessOnRemoveAsync()
        {
            return Container
                .EffectAsync(async c =>
                {
                    var store = c.GetRequiredService<ICacheStoreProvider<InDistributed>>().CacheStore;
                    var result = await store.RemoveAsync(Key, OperationOptions);
                    result.Should().BeSuccessfulResult();
                });
        }
    }
}