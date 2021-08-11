using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warehouse.Core.Interfaces;
using Warehouse.Infrastructure.DataAccess.Repositories;

namespace Warehouse.Web.Infrastructure.Autofac.Modules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>))
                .AsImplementedInterfaces();
        }
    }
}
