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
                .NotEmpty();
            RuleFor(x => x.Description).Length(0, 512)
                .NotEmpty();
            RuleFor(x => x.Price).GreaterThan((decimal)0)
                .NotEmpty();
            RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
