using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
    }
}
