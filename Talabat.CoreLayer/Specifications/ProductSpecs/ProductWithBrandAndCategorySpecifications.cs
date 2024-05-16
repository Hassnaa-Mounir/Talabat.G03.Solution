using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;

namespace Talabat.CoreLayer.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        //public ProductWithBrandAndCategorySpecifications() : base()
        //{
        //    AddInclude();
        //}
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(p =>

                (!specParams.BrandId.HasValue || p.ProductBrandId == specParams.BrandId.Value) &&
                 (!specParams.CategoryId.HasValue || p.ProductTypeId == specParams.CategoryId.Value)
            ) 
        {
            AddInclude();
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        //OrderBy = p => p.Price;
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else AddOrderBy(p => p.Name);
            ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);
        }

        public ProductWithBrandAndCategorySpecifications(int id) : base(P => P.Id == id)
        {
            AddInclude();
        }
        private void AddInclude()
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
