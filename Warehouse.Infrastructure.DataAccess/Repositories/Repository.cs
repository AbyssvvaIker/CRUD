using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Core.Entities;
using Warehouse.Core.Interfaces;

namespace Warehouse.Infrastructure.DataAccess.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity, new()
    {
        protected readonly DataContext DataContext;
        protected Repository(DataContext dataContext)
        {
            DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public virtual async Task<IEnumerable<T>> GetAllActiveAsync()
        {
            return await DataContext.Set<T>()
                .Where(x => x.IsActive)
                .ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await DataContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await DataContext.Set<T>()
                .AddAsync(entity);
            return entity;
        }

        public virtual void Delete(T entity)
        {
            entity.IsActive = false; 
        }

        public virtual async Task SaveChangesAsync()
        {
            await DataContext
                .SaveChangesAsync();
        }


    }
}
