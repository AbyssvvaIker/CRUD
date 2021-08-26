using System.Linq;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Warehouse.Core;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.Core
{
    public static class ActionResultExtensions
    {
        public static ActionResultAssertions Should(this IActionResult actionResult)
        {
            return new ActionResultAssertions(actionResult);
        }
    }

    public class ActionResultAssertions : ReferenceTypeAssertions<IActionResult, ActionResultAssertions>
    {
        public ActionResultAssertions(IActionResult actionResult) : base(actionResult)
        {
        }

        protected override string Identifier => "actionResult";

        public ActionResultAssertions BeOk<T>(T value, string because = "", params object[] becauseArgs)
        {
            var subject = Subject.As<ObjectResult>();
            var subjectValue = subject.Value.As<Result<T>>(); //subjectValue is null when running tests(?)

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject != null)
                .FailWith("The ObjectResult can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject.StatusCode == 200)
                .FailWith($"The StatusCode should be 200, but is {subject.StatusCode}");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Success == true)
                .FailWith("The Result.Success should be true.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value != null)
                .FailWith("The Result.Value can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value.Equals(value))
                .FailWith("The Value should be the same as expected.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors != null)
                .FailWith("The Errors can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors.Any() == false)
                .FailWith("The Errors shouldn't have errors.");

            return this;
        }

        public ActionResultAssertions BeCreatedAtAction<T>(T value, string because = "", params object[] becauseArgs)
        {
            var subject = Subject.As<ObjectResult>();
            var subjectValue = subject.Value.As<Result<T>>(); //subjectValue is null when running tests(?)

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject != null)
                .FailWith("The ObjectResult can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject.StatusCode == 201)
                .FailWith($"The StatusCode should be 201, but is {subject.StatusCode}");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Success == true)
                .FailWith("The Result.Success should be true.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value != null)
                .FailWith("The Result.Value can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value.Equals(value))
                .FailWith("The Value should be the same as expected.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors != null)
                .FailWith("The Errors can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors.Any() == false)
                .FailWith("The Errors shouldn't have errors.");

            return this;
        }

        public ActionResultAssertions BeNotFound<T>(string property, string message, string because = "", params object[] becauseArgs)
        {
            var subject = Subject.As<ObjectResult>();
            var subjectValue = subject.Value.As<Result<T>>(); //subjectValue is null when running tests(?)

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject != null)
                .FailWith("The ObjectResult can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject.StatusCode == 404)
                .FailWith($"The StatusCode should be 404, but is {subject.StatusCode}");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Success == false)
                .FailWith("The Result.Success should be false.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value == null)
                .FailWith("The Result.Value should be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors != null)
                .FailWith("The Errors can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors.Any())
                .FailWith("The Errors should have errors.");

            var error = subjectValue.Errors.FirstOrDefault();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(error.Message == message)
                .FailWith($"The Message should be '{message}', but is '{error.Message}'");

            return this;
        }

        public ActionResultAssertions BeBadRequest<T>(string property, string message, string because = "", params object[] becauseArgs)
        {
            var subject = Subject.As<ObjectResult>();
            var subjectValue = subject.Value.As<Result<T>>(); //subjectValue is null when running tests(?)

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject != null)
                .FailWith("The ObjectResult can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subject.StatusCode == 400)
                .FailWith($"The StatusCode should be 400, but is {subject.StatusCode}");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Success == false)
                .FailWith("The Result.Success should be false.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Value == null)
                .FailWith("The Result.Value should be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors != null)
                .FailWith("The Errors can't be null.");

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(subjectValue.Errors.Any())
                .FailWith("The Errors should have errors.");

            var error = subjectValue.Errors.FirstOrDefault();

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(error.Message == message)
                .FailWith($"The Message for property '{property}' should be '{message}', but is {error.Message}");

            return this;
        }
    }
}
