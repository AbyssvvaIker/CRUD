using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Interfaces;

namespace Warehouse.Api.Infrastructure.Autofac.Modules
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(ILogic).Assembly)
                .Where(t => typeof(ILogic).IsAssignableFrom(t))
                .AsImplementedInterfaces();
        }
    }
}
