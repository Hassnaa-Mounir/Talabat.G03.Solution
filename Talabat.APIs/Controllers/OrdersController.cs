using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.CoreLayer.Entities.Idintity;
using Talabat.CoreLayer.Services;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrdersController(
            IOrderService orderService,
            IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost] // POST /api/Orders
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var address = mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order == null) return BadRequest(new ApiResponse(400));

            return Ok(order);
        }
        [HttpGet] // GET : /api/Orders
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderForUser(string email)
        {
            var orders = await orderService.GetOrdersForUserAsync(email);
            return Ok(orders);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // GET : /api/Orders/1
        public async Task<ActionResult<Order>> GetOrderForUser(int id, string email)
        {
            var order = await orderService.GetOrderByIdForUserAsync(id, email);

            if (order == null) return NotFound(new ApiResponse(404));

            return Ok(order);
        }
    }
}
