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


        //Get Product By Id
    
    }
}
