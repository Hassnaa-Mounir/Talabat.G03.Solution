using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> genericRepository;

        public ProductsController(IGenericRepository<Product> genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        //Get All Products

        //BaseURL/api/controller ----- determine verb method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()  //ActionResult<IEnumerable<Product>> specific for frontend shaped of response
        {                                                                    // to helped frontend that consume data that must display
            var products = await genericRepository.GetAllAsync();

            ///OkObjectResult okObject =new OkObjectResult(products);
            ///return okObject;

            //using helper method
            return Ok(products);

        }


      

    }
}
