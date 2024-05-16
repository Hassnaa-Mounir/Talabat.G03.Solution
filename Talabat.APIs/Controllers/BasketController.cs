using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        [HttpGet] // GET: /api/basket?id
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]// POST: /api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var createdOrUpdatedBasket = await basketRepository.UpdateBasketAsync(basket);
            if (createdOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(createdOrUpdatedBasket);

        }
        [HttpDelete]// DELETE : /api/basket
        public async Task DeleteBasket(string id)
        {
            await basketRepository.DeleteBasketAsync(id);
        }
    }
}
