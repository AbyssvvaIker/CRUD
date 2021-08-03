using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.WebMvc.Models;

namespace Warehouse.WebMvc.Data
{
    public class MvcCategoryContext : DbContext
    {
        public MvcCategoryContext(DbContextOptions<MvcCategoryContext> options)
            : base(options)
        {

        }

        public DbSet<Category> Movie { get; set; }
    }
}
