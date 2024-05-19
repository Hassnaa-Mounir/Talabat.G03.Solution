using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities.Order_Aggregate;

namespace Talabat.RepositoryLayer.Data.Configurations.Order_Config
{
    internal class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.OwnsOne(orderItem => orderItem.Product, product => product.WithOwner());

            builder.Property(orderItem => orderItem.Price)
                .HasColumnType("decimal(12,2)");
        }
    }
}
