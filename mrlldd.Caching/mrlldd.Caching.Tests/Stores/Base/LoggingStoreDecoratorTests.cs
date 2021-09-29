using System;
using System.Linq.Expressions;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using mrlldd.Caching.Flags;
using mrlldd.Caching.Stores;
using mrlldd.Caching.Tests.TestUtilities.Extensions;
using NUnit.Framework;

namespace mrlldd.Caching.Tests.Stores.Base
{
    public abstract class LoggingStoreDecoratorTests : StoreDecoratorTest
    {
        protected const LogLevel DefaultLogLevel = LogLevel.Information;

        private readonly Expression<Action<ILogger<ICacheStore<InVoid>>>> loggerSetup =
            x => x.Log(
                It.Is<LogLevel>(l => l == DefaultLogLevel),
                It.Is<EventId>(e => e.Id == 0),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            );
        
        protected abstract Times Times { get; }

        protected override void FillContainer(IContainer container)
        {
            base.FillContainer(container);
            var logger = container
                .GetRequiredService<ILoggerFactory>()
                .CreateLogger<ICacheStore<InVoid>>();
            container.AddMock<ILoggerFactory>(MockRepository);
            container.AddMock<ILogger<ICacheStore<InVoid>>>(MockRepository);
            var loggerMock = container.GetRequiredService<Mock<ILogger<ICacheStore<InVoid>>>>();
            loggerMock
                .Setup(loggerSetup)
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel =
                        (LogLevel)invocation
                            .Arguments[0]; // The first two will always be whatever is specified in the setup above
                    var eventId =
                        (EventId)invocation.Arguments[1]; // so I'm not sure you would ever want to actually use them
                    var state = invocation.Arguments[2];
                    var exception = (Exception?)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception })!;
                    logger.Log(logLevel, eventId, logMessage);
                }))
                .Verifiable();
        }

        [TearDown]
        public void Teardown()
            => Container
                .GetRequiredService<Mock<ILogger<ICacheStore<InVoid>>>>()
                .Verify(loggerSetup, Times);
    }
}