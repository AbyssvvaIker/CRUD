using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    class ProductRepository : Repository<Product>, IProductRepository 
    {
        public ProductRepository(DataContext db) : base(db)
        {

        }

        public async Task DeleteByCategoryIdAsync(Guid categoryId)
        {
            var products =  await DataContext.Set<Product>()
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
            foreach(var product in products)
            {
                product.IsActive = false;
            }
        }
    }
}
