using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> genericRepository;
        private readonly IMapper mapper;

        public ProductsController(IGenericRepository<Product> genericRepository, IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this.mapper = mapper;
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
            var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

            //using helper method
            return Ok(result);

        }

        //Get Product By Id

        [HttpGet("{id}")] // you called that verb have variable
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await genericRepository.GetByIdAsync(id);
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            if (product is null)
                return NotFound(/*new { Message = "Not Found", StatusCode = 404 }*/new ApiResponse(404)); // 404
            var result = mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(result);
        }
   
}
}
