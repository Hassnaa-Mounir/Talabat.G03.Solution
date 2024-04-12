using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Entities
{
    //must named properties same as structure to can consume with angular or front
    public class Product : BaseModel
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price{ get; set; }
        //forienkey of relation productBrand
        [ForeignKey(nameof(ProductBrand))]
        public int ProductBrandId { get; set; }
        [ForeignKey(nameof(ProductType))]
        public int ProductTypeId { get; set; }
        // navigational property
        public ProductBrand ProductBrand { get; set; } = null!;
        // navigational property
        public ProductType ProductType { get; set; } = null!;   
    }
}
