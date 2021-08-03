using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//using Warehouse.WebMvc.Models;

namespace Warehouse.Infrastructure.DataAccess
{
    public class MvcCategoryContext : DbContext
    {
        public MvcCategoryContext(DbContextOptions<MvcCategoryContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Category { get; set; }
    }
}
