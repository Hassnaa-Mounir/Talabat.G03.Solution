using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities.Order_Aggregate;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Services;
using Stripe;
using Talabat.CoreLayer.Specifications.OrderSpec;

namespace Talabat.ServicesLayer.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepo;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork
            )
        {
            this.configuration = configuration;
            this.basketRepo = basketRepo;
            this.unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = shippingPrice;
            }

            if (basket.Items?.Count > 0)
            {
                var prouductRepo = unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await prouductRepo.GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent = new PaymentIntent();

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options); // Integration with stripe 

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else // update Existing Payment Intent 
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepo.UpdateBasketAsync(basket);
            return basket;


        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.Repository<Order>();

            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(spec);

            if (order == null) return null;

            if (isPaid)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            orderRepo.Update(order);

            await unitOfWork.CompleteAsync();

            return order;
        }
    }
}
