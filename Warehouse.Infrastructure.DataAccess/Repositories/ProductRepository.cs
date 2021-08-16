using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;
using Warehouse.Core.Interfaces.Repositories;
using Z.EntityFramework.Plus;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    class ProductRepository : Repository<Product>, IProductRepository 
    {
        public ProductRepository(DataContext db) : base(db)
        {

        }

        public async Task DeleteByCategoryIdAsync(Guid categoryId)
        {
            await DataContext.Set<Product>()
                .Where(x => x.CategoryId == categoryId)
                .UpdateAsync(x => new Product() { IsActive = false });

        }

        public override async Task<Product> GetByIdAsync(Guid id)
        {
            return await DataContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(predicate => predicate.Id == id && predicate.IsActive);
        }
    }
}
