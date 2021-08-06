using System;
using System.Threading;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Functional.Object.Extensions;
using ImTools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
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
                .AddStores()
                .Effect(x => Container.Populate(x));
            Container.Register<IDistributedCache, NoOpDistributedCache>();
            FillContainer(Container);
        }

        protected virtual void FillContainer(IContainer container)
        {
        }
    }
}