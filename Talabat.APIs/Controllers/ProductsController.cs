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
            
            var spec = new ProductWithBrandAndCategorySpecifications();

            var products = await genericRepository.GetAllWithSpecAsync(spec);
            ///OkObjectResult okObject =new OkObjectResult(products);
            ///return okObject;

            //using helper method
            return Ok(products);

        }

        //Get Product By Id

        [HttpGet("{id}")] // you called that verb have variable
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);

            return Ok(product);
        }


    }
}
