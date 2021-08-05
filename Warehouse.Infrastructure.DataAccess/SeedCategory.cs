using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Warehouse.Infrastructure.DataAccess;
using Warehouse.Core.Entities;

namespace Warehouse.Infrastructure.DataAccess
{
    public static class SeedCategory
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<DataContext>>()))
            {
                if (context.Categories.Any())
                {
                    return;
                }

                context.Categories.AddRange(
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
