using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.Core.Entities;

namespace Warehouse.Infrastructure.DataAccess.EntityConfigurations
{
    class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);
            //Category is formatted in CategoryEntityConfiguration
            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(256);
            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(9,2)");
        }
    }
}
