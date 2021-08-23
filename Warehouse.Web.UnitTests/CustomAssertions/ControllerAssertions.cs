using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Warehouse.Web.Controllers;

namespace Warehouse.Web.UnitTests.CustomAssertions
{
    public class ControllerAssertions : ReferenceTypeAssertions<Controller, ControllerAssertions>
    {
        protected override string Identifier => "controller";

        public ControllerAssertions(Controller controller) : base(controller) { }

        public ControllerAssertions HasError(string property, string message, string because = "",
            params object[] becauseArgs)
        {
            var state = Subject.ModelState.FirstOrDefault(m => m.Key == property);
            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(state.Key == property)
            .FailWith($"The controller should have error for property: {property}.");

            Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(state.Value.Errors.Any(e => e.ErrorMessage == message))
            .FailWith($"The controller should have error with message: {message}.");
            return this;
        }
    }

    public static class ControllerExtensions
    {
        public static ControllerAssertions Should(this Controller instance)
        {
            return new ControllerAssertions(instance);
        }
    }
}
