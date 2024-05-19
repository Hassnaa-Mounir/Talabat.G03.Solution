﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using Talabat.CoreLayer.Entities.Order_Aggregate;
using Order = Talabat.CoreLayer.Entities.Order_Aggregate.Order;

namespace Talabat.RepositoryLayer.Data.Configurations.Order_Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(order => order.Status)
                .HasConversion(
                (Ostatus) => Ostatus.ToString(),
                (Ostatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                );

            builder.Property(order => order.Subtotal)
                .HasColumnType("decimal(12,2)");
        }
    }
}
