using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    class ProductRepository : Repository<Product>, IProductRepository 
    {
        public ProductRepository(DataContext db) : base(db)
        {

        }

        public virtual async Task<IEnumerable<string>> GetUniqueCategories()
        {
            var result = DataContext.Categories
                .Select(c => c.Name)
                .Distinct()
                .ToList();
            return result;
        }
    }
}
