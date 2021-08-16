using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Validators
{
    public class CategoryValidator :AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty()
                .Length(2, 32)
                .WithMessage("{PropertyName}, it has to be {MinLength} to {MaxLength} characters long");
        }
    }
}
