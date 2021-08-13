using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Interfaces
{
    public interface ICategoryLogic : ILogic
    {
        Task<Result<IEnumerable<Category>>> GetAllActiveAsync();
        Task<Result<Category>> GetByIdAsync(Guid id);
        Task<Result<Category>> AddAsync(Category category);
        Task<Result<Category>> UpdateAsync(Category category);
        Task<Result> DeleteAsync(Category category);
    }
}
