using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
    }
}
