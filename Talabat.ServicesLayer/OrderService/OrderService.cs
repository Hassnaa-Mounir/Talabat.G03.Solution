using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Entities.Order_Aggregate;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Services;
using Talabat.CoreLayer.Specifications.OrderSpec;

namespace Talabat.ServicesLayer.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepo;
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<DeliveryMethod> deliveryMethodRepo;
        private readonly IGenericRepository<Order> orderRepo;

        public OrderService(IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<Order> orderRepo)
        {
            this.basketRepo = basketRepo;
            this.productRepo = productRepo;
            this.deliveryMethodRepo = deliveryMethodRepo;
            this.orderRepo = orderRepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1.Get Basket From Baskets Repo

            var basket = await basketRepo.GetBasketAsync(basketId);


            // 2. Get Selected Items at Basket From Products Repo

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);

                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            //var deliveryMethod = await deliveryMethodRepo.GetAsync(deliveryMethodId);
            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 5. Create Order 

            var order = new Order(
                buyerEmail: buyerEmail,
                shippingAddress: shippingAddress,
               deliveryMethod: deliveryMethod,
                items: orderItems,
                subtotal: subtotal
                );

            orderRepo.Add(order);

            // 6. Save To Database [TODO]

            return order;
        }
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
               return unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var orderRepo = unitOfWork.Repository<Order>();

            var orderSpec = new OrderSpecifications(orderId, buyerEmail);

            var order = orderRepo.GetWithSpecAsync(orderSpec);
            return order;
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var orderRepo = unitOfWork.Repository<Order>();

            var spec = new OrderSpecifications(buyerEmail);

            var orders = await orderRepo.GetAllWithSpecAsync(spec);

            return orders;
        }
    }
}
