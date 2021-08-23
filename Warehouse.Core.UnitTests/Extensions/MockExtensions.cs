using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Warehouse.Core.UnitTests.Extensions
{
    public static class MockExtensions
    {
        public static void SetValidationSuccess<T>(this Mock<IValidator<T>> validator)
        {
            validator.Setup(x => x.Validate(It.IsAny<T>()))
                .Returns(new ValidationResult());
        }

        public static void SetValidationFailure<T>(this Mock<IValidator<T>> validator, string validatedProperty,
            string errorMessage)
        {
            validator.Setup(x => x.Validate(It.IsAny<T>()))
                .Returns(new ValidationResult(new List<ValidationFailure>()
                {
                    new ValidationFailure(validatedProperty, errorMessage)
                }));
        }
    }
}
