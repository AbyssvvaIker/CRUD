using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;

namespace Warehouse.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllActiveAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();


    }
}
