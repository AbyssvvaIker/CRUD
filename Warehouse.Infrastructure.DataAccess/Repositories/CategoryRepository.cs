using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DataContext db) : base(db)
        {

        }
    }
}
