﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        //[EnumMember]
        Pending,
        PaymentReceived,
        PaymentFailed
    }
}