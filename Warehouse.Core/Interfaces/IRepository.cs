using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Core.Interfaces
{
    public interface IRepository<T> 
    {
        Task<IEnumerable<T>> GetAllActiveAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();


    }
}
