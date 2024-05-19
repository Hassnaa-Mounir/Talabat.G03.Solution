using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Services;
using Talabat.CoreLayer.Specifications.ProductSpecs;

namespace Talabat.ServicesLayer
{

    public class ProductService : IproductService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
            => await unitOfWork.Repository<ProductBrand>().GetAllAsync();

        public async Task<IReadOnlyList<ProductType>> GetCategoriesAsync()
            => await unitOfWork.Repository<ProductType>().GetAllAsync();

        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);
            var products = await unitOfWork.Repository<Product>().GetWithSpecAsync(spec);

            return products;
        }

        public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var countSpec = new ProductWithFilterationForCountSpecification(specParams);

            var count = await unitOfWork.Repository<Product>().GetCountAsync(countSpec);

            return count;
        }
    }
}
