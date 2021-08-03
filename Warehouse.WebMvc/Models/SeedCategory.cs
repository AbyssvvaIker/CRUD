using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Warehouse.Infrastructure.DataAccess;

namespace Warehouse.WebMvc.Models
{
    public static class SeedCategory
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcCategoryContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MvcCategoryContext>>()))
            {
                if (context.Category.Any())
                {
                    return;
                }

                context.Category.AddRange(
                    new Category
                    {
                        Name = "Food"
                    },

                    new Category
                    {
                        Name = "PC"
                    },

                    new Category
                    {
                        Name = "Car"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
