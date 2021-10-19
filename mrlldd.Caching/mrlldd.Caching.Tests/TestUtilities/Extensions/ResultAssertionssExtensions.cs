using System;
using FluentAssertions;
using FluentAssertions.Primitives;
using Functional.Result;

namespace mrlldd.Caching.Tests.TestUtilities.Extensions
{
    public static class ResultAssertionssExtensions
    {
        public static AndWhichConstraint<ObjectAssertions, Success<T>> BeSuccessfulResult<T>(
            this ObjectAssertions should)
        {
            should
                .NotBeNull()
                .And.BeAssignableTo<Result<T>>()
                .Which.Successful.Should().BeTrue();
            return should.BeOfType<Success<T>>();
        }

        public static AndWhichConstraint<ObjectAssertions, Success> BeSuccessfulResult(this ObjectAssertions should)
        {
            should
                .NotBeNull()
                .And.BeAssignableTo<Result>()
                .Which.Successful.Should().BeTrue();
            return should.BeOfType<Success>();
        }

        public static AndWhichConstraint<ObjectAssertions, Fail<T>> BeFailResult<T>(this ObjectAssertions should)
        {
            should
                .NotBeNull()
                .And.BeAssignableTo<Result<T>>()
                .Which.Successful.Should().BeFalse();
            return should.BeOfType<Fail<T>>();
        }

        public static AndWhichConstraint<ObjectAssertions, Fail> BeFailResult(this ObjectAssertions should)
        {
            should
                .NotBeNull()
                .And.BeAssignableTo<Result>()
                .Which.Successful.Should().BeFalse();
            return should.BeOfType<Fail>();
        }

        public static AndWhichConstraint<ObjectAssertions, TException> WithException<TException>(
            this AndWhichConstraint<ObjectAssertions, Fail> fail) where TException : Exception
        {
            return fail.Which.Exception
                .Should()
                .NotBeNull()
                .And.BeOfType<TException>();
        }

        public static AndWhichConstraint<ObjectAssertions, TException> WithException<T, TException>(
            this AndWhichConstraint<ObjectAssertions, Fail<T>> fail) where TException : Exception
        {
            return fail.Which.Exception
                .Should()
                .NotBeNull()
                .And.BeOfType<TException>();
        }
    }
}