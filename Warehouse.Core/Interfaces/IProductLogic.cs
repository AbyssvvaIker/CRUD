using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;


namespace Warehouse.Core.Interfaces
{
    interface IProductLogic : ILogic
    {
        Task<Result<IEnumerable<Product>>> GetAllActiveAsync();

        Task<Result<Product>> GetByIdAsync(Guid id);

        Task<Result<Product>> AddAsync(Product product);
        Task<Result<Product>> UpdateAsync(Product product);
        Task<Result> DeleteAsync(Product product);
    }
}
