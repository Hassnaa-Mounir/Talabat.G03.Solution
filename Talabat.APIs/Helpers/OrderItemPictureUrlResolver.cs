﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.CoreLayer.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{configuration["ApiBaseUrl"]}/{source.Product.PictureUrl}";


            return string.Empty;
        }
    }
}
