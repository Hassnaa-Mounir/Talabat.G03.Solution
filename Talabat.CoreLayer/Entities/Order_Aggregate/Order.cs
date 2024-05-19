using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Entities.Order_Aggregate
{
    public class Order : BaseModel
    {
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }

        //public int DeliveryMethodId { get; set; } // Foreign Key 

        //  public DeliveryMethod DeliveryMethod { get; set; } // Navigational Property [One]

        public DeliveryMethod? DeliveryMethod { get; set; } // Navigational Property [One]

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property [many]

        public decimal Subtotal { get; set; }

        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost;

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } = string.Empty;

    }
}
