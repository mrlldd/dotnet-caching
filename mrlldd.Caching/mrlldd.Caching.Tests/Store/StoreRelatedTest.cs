using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace mrlldd.Caching.Tests.Store
{
    public class StoreRelatedTest : Test
    {
        protected const string Key = "key";

        protected static void CallsSpecific<T>(Mock<T> mock,
            Expression<Action<T>> setup, Action action)
            where T : class
        {
            mock.Setup(setup)
                .Verifiable();
            action();
            mock.Verify(setup, Times.Once);
        }

        protected static void CallsSpecific<T, TResult>(Mock<T> mock,
            Expression<Func<T, TResult>> setup, Action action)
            where T : class
        {
            mock.Setup(setup)
                .Verifiable();
            action();
            mock.Verify(setup, Times.Once);
        }

        protected static async Task CallsSpecificAsync<T, TResult>(Mock<T> mock,
            Expression<Func<T, TResult>> setup, Func<Task> asyncAction) where T : class
        {
            mock.Setup(setup).Verifiable();
            await asyncAction();
            mock.Verify(setup, Times.Once);
        }

        protected static async Task CallsSpecificAsync<T>(Mock<T> mock,
            Expression<Action<T>> setup, Func<Task> asyncAction)
            where T : class
        {
            mock.Setup(setup).Verifiable();
            await asyncAction();
            mock.Verify(setup, Times.Once);
        }
    }
}