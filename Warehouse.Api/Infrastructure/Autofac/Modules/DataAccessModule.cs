using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;
using Warehouse.Infrastructure.DataAccess.Repositories;

namespace Warehouse.Api.Infrastructure.Autofac.Modules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .AsImplementedInterfaces();
        }
    }
}
