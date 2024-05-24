using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities.Order_Aggregate;

namespace Talabat.CoreLayer.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpecification : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
