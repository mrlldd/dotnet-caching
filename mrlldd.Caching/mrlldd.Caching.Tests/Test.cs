using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Functional.Object.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using mrlldd.Caching.Caches;
using NUnit.Framework;

namespace mrlldd.Caching.Tests
{
    [TestFixture]
    public class Test
    {
        protected IContainer Container { get; private set; }

        protected MockRepository MockRepository { get; } = new MockRepository(MockBehavior.Loose);
        
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
                .Effect(x => Container.Populate(x));
            Container.Register<IDistributedCache, NoOpDistributedCache>();
            FillContainer(Container);
        }
        
        protected virtual void FillContainer(IContainer container) {}
    }
}