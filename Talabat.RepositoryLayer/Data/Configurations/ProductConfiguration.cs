using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;

namespace Talabat.RepositoryLayer.Data.Configurations
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
           builder.HasOne(p=>p.ProductBrand).WithMany(p=>p.Products).HasForeignKey(p=>p.ProductBrandId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p=>p.ProductType).WithMany(p=>p.Products).HasForeignKey(p=>p.ProductTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.PictureUrl).IsRequired();
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        }
    }
}
