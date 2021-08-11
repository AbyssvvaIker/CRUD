using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Interfaces
{
    interface ICategoryLogic : ILogic
    {
        Task<Result<IEnumerable<Category>>> GetAllActiveAsync();
        Task<Result<Category>> GetByIdAsync(Guid id);

        Task<Result<Category>> AddAsync(Category product);
        Task<Result<Category>> UpdateAsync(Category product);
        Task<Result> DeleteAsync(Category product);
    }
}
