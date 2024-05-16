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
        public ProductWithBrandAndCategorySpecifications(string sort)
        {
            AddInclude();
            if (!string.IsNullOrEmpty(sort))
            {

            }
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
