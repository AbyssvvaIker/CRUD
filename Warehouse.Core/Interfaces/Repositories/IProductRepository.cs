using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task DeleteByCategoryIdAsync(Guid id);
    }
}
