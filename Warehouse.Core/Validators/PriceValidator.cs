using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Core.Validators
{
    public class PriceValidator<T, Tproperty> : PropertyValidator<T, Tproperty>
    {
        public override string Name => "PriceValidator";

        public override bool IsValid(ValidationContext<T> context, Tproperty value)
        {
            var price = value is decimal ? Convert.ToDecimal(value) : 0;
            return PriceValidate(price);
        }

        private bool PriceValidate(decimal price)
        {
            if(price <= decimal.Zero)
            {
                return false;
            }
            price *= 100;
            if(price % 1 != 0)
            {
                return false;
            }
            return true;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
        {
            return "{PropertyName} has invalid format";
        }

    }
}
