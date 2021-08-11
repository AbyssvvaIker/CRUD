using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Core.Logic
{
    class CategoryLogic : ICategoryLogic
    {
        public Task<Result<Category>> AddAsync(Category product)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Category product)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Category>>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Category>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Category>> UpdateAsync(Category product)
        {
            throw new NotImplementedException();
        }
    }
}
