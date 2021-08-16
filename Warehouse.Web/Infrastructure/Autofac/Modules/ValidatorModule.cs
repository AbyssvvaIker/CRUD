using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warehouse.Web.Infrastructure.Autofac.Modules
{
    public class ValidatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ValidatorModule).Assembly)
                .Where(t => t.Name.EndsWith("Validator"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
