using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using mrlldd.Caching.Caches;
using mrlldd.Caching.Caches.Internal;
using mrlldd.Caching.Decoration.Internal.Logging.Actions;
using mrlldd.Caching.Decoration.Internal.Logging.Performance;
using mrlldd.Caching.Logging;
using mrlldd.Caching.Logging.Internal;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Stores.Decoration;
using mrlldd.Caching.Stores.Internal;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    public class Test
    {
        protected IContainer Container { get; private set; }

        protected MockRepository MockRepository { get; } = new(MockBehavior.Loose);

        [SetUp]
        protected void Setup()
        {
            Container = new Container(r =>
                r.WithoutFastExpressionCompiler()
                    .WithDefaultIfAlreadyRegistered(IfAlreadyRegistered.Replace)
                    .WithTrackingDisposableTransients()
                    .With(FactoryMethod.ConstructorWithResolvableArguments));
            new ServiceCollection()
                .AddLogging(x => x.AddConsole().AddFilter(level => level >= LogLevel.Debug))
                .AddMemoryCache()
                .AddCachingStores()
                .AddSingleton<ICachingPerformanceLoggingOptions>(new CachingPerformanceLoggingOptions(LogLevel.Information))
                .AddSingleton<ICachingActionsLoggingOptions>(new CachingActionsLoggingOptions(LogLevel.Information, LogLevel.Error))
                .AddSingleton<ICacheOptions>(new NullDefaultCacheOptions())
                .Effect(x => Container.Populate(x));
            Container.Register<ICacheStoreDecorator, PerformanceLoggingCacheStoreDecorator>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
            Container.Register<ICacheStoreDecorator, ActionsLoggingCacheStoreDecorator>(ifAlreadyRegistered: IfAlreadyRegistered.AppendNewImplementation);
            Container.Register<IDistributedCache, NoOpDistributedCache>();
            Container.Register<IStoreOperationProvider, StoreOperationProvider>();
            FillContainer(Container);
        }

        protected virtual void FillContainer(IContainer container)
        {
        }
    }
}