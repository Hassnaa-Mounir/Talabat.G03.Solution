﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Entities.Order_Aggregate
{
    public class OrderItem : BaseModel
    {
        public ProductItemOrdered Product { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
