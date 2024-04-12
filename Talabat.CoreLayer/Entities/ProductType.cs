using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Entities
{
    internal class ProductType : BaseModel
    {
        public string Name { get; set; } = null!;

        // navigational property
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}
