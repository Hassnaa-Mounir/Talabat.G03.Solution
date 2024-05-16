using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Specifications.ProductSpecs;
using Talabat.RepositoryLayer;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> productsRepo;
        private readonly IMapper mapper;
        private readonly IGenericRepository<ProductBrand> brandsRepo;
        private readonly IGenericRepository<ProductType> categoriesRepo;

        public ProductsController(
             IGenericRepository<Product> productsRepo,
             IMapper mapper,
             IGenericRepository<ProductBrand> brandsRepo,
             IGenericRepository<ProductType> categoriesRepo
             )
        {
            this.productsRepo = productsRepo;
            this.mapper = mapper;
            this.brandsRepo = brandsRepo;
            this.categoriesRepo = categoriesRepo;
        }
        //Get All Products

        //BaseURL/api/controller ----- determine verb method
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()  //ActionResult<IEnumerable<Product>> specific for frontend shaped of response
        {                                                                    // to helped frontend that consume data that must display
            
            var spec = new ProductWithBrandAndCategorySpecifications();

            var products = await productsRepo.GetAllWithSpecAsync(spec);
            ///OkObjectResult okObject =new OkObjectResult(products);
            ///return okObject;
            var result = mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);

            //using helper method
            return Ok(result);

        }

        //Get Product By Id

        [HttpGet("{id}")] // you called that verb have variable
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productsRepo.GetByIdAsync(id);
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            if (product is null)
                return NotFound(/*new { Message = "Not Found", StatusCode = 404 }*/new ApiResponse(404)); // 404
            var result = mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(result);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<ProductBrand>>> GetBrands()
        {
            var brands = await brandsRepo.GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductType>>> GetCategories()
        {
            var categories = await categoriesRepo.GetAllAsync();
            return Ok(categories);
        }
    }
}
