using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.APIs.Helpers;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Services;
using Talabat.CoreLayer.Specifications.ProductSpecs;
using Talabat.RepositoryLayer;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        //private readonly IGenericRepository<Product> productsRepo;
        private readonly IMapper mapper;
        //private readonly IGenericRepository<ProductBrand> brandsRepo;
        //private readonly IGenericRepository<ProductType> categoriesRepo;
        private readonly IproductService productService;
        ////private readonly IGenaricRepository<Product> productsRepo;
        ////private readonly IGenaricRepository<ProductBrand> brandsRepo;
        ////private readonly IGenaricRepository<ProductCategory> categoriesRepo;

        public ProductsController(IMapper mapper,
            IproductService productService
             )
        {
           // this.productsRepo = productsRepo;
            this.mapper = mapper;
            // this.brandsRepo = brandsRepo;
            // this.categoriesRepo = categoriesRepo;
            this.productService = productService;

        }
        //Get All Products

        //BaseURL/api/controller ----- determine verb method
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)  //ActionResult<IEnumerable<Product>> specific for frontend shaped of response
        {                                                                    // to helped frontend that consume data that must display
            
           // var spec = new ProductWithBrandAndCategorySpecifications(specParams);

           // var products = await productsRepo.GetAllWithSpecAsync(spec);
            var products = await productService.GetProductsAsync(specParams);

            var count = await productService.GetCountAsync(specParams);
            ///OkObjectResult okObject =new OkObjectResult(products);
            ///return okObject;
            var result = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>((IReadOnlyList<Product>)products);

            //using helper method
            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

           // var countSpec = new ProductWithFilterationForCountSpecification(specParams);

            //var count = await productsRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, count, data));

        }

        //Get Product By Id

        [HttpGet("{id}")] // you called that verb have variable
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productService.GetProductAsync(id);

            if (product is null)
                return NotFound(/*new { Message = "Not Found", StatusCode = 404 }*/new ApiResponse(404)); // 404
            var result = mapper.Map<Product, ProductToReturnDto>(product);
            return Ok(mapper.Map<Product, ProductToReturnDto>(product)); // 200 

            
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await productService.GetBrandsAsync(); return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetCategories()
        {
            var categories = await productService.GetCategoriesAsync(); return Ok(categories);
        }
    }
}
