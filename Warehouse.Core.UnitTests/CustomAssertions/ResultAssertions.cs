using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System.Linq;

namespace Warehouse.Core.UnitTests.CustomAssertions
{
    public class ResultAssertions<T> : ReferenceTypeAssertions<Result<T>, ResultAssertions<T>>
    {
        protected override string Identifier => "result";
        public ResultAssertions(Result<T> result)
            : base(result)
        {
            //Subject = result;
        }
        public ResultAssertions<T> BeSuccess(T value, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject != null)
                .FailWith("The result cannot be null");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Success)
                .FailWith("The Success should be true");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Value != null)
                .FailWith("Value cannot be null");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Value.Equals(value))
                .FailWith("The Value should be same as expected");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Errors.Any() == false)
                .FailWith("The IEnumerable<Errors> should be null");

            return this;
        }
        public ResultAssertions<T> BeFailure(string property, string message, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject != null)
                .FailWith("The result can't be null");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Success == false)
                .FailWith("The Subject should be false");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Value == null && (Subject.Success == false))
                .FailWith("The Value should be null");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject.Errors.Any())
                .FailWith("The Errors should have errors");

            var error = Subject.Errors.FirstOrDefault(e => e.PropertyName == property);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(error != null)
                .FailWith($"The Errors should contain error for property '{property}'");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(error.Message == message)
                .FailWith($"The Message for property '{property}' should be '{message}'");

            return this;
        }

        public ResultAssertions<T> BeFailure(string message,
                string because = "",
                params object[] becauseArgs)
        {
            BeFailure(string.Empty, message, because, becauseArgs);

            return this;
        }

    }

    public static class ResultExtensions
    {
        public static ResultAssertions<T> Should<T>(this Result<T> instance)
        {
            return new ResultAssertions<T>(instance);
        }
    }

}
