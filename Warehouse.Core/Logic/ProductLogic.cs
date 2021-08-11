using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Core.Logic
{
    class ProductLogic : IProductLogic
    {
        public Task<Result<Product>> AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<Product>>> GetAllActiveAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Product>> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Product>> UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
