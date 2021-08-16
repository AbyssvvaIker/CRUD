using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).Length(2, 32)
                .NotEmpty()
                .WithMessage("{PropertyName}, it has to be from {MinLength} to {MaxLength} long");
            RuleFor(x => x.Description).Length(1, 512)
                .NotEmpty()
                .WithMessage("{PropertyName}, it has to be from {MinLength} to {MaxLength} long");
            RuleFor(x => x.Price).GreaterThan((decimal)0)
                .NotEmpty()
                .WithMessage("{PropertyName}, it has to be greater than {ComparisionValue}");
            RuleFor(x => x.Category).NotEmpty();
        }
    }
}
