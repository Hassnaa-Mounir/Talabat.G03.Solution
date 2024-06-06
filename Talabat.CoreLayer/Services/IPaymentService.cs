using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Entities.Order_Aggregate;

namespace Talabat.CoreLayer.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order> UpdateOrderStatus(string paymentIntentId, bool isPaid);
    }
}
