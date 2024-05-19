using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Specifications.ProductSpecs;

namespace Talabat.CoreLayer.Services
{
    public interface IproductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams);

        Task<Product?> GetProductAsync(int productId);

        Task<int> GetCountAsync(ProductSpecParams specParams);

        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();

        Task<IReadOnlyList<ProductType>> GetCategoriesAsync();

    }
}
