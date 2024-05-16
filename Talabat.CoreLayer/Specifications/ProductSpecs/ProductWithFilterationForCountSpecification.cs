using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;

namespace Talabat.CoreLayer.Specifications.ProductSpecs
{
    public class ProductWithFilterationForCountSpecification : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams specParams)
            : base(p =>
                 (!specParams.BrandId.HasValue || p.ProductBrandId == specParams.BrandId.Value) &&
                 (!specParams.CategoryId.HasValue || p.ProductTypeId == specParams.CategoryId.Value)
            )
        {

        }
    }
}
