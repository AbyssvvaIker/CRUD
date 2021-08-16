using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Warehouse.Core.Validators;
using Warehouse.Core.Interfaces;

namespace Warehouse.Web.Infrastructure.Autofac.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .AsImplementedInterfaces();
        }
    }
}
