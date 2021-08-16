﻿using FluentValidation;
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
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(2, 32);
            RuleFor(x => x.Description).Length(0, 512);
            RuleFor(x => x.Price).GreaterThan(0);
            RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
