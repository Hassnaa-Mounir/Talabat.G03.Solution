using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;

namespace Talabat.RepositoryLayer.Data
{
    public static class StoreContextDataSeed
    {

        //Add DataSeed in DataBase

        public static async Task SeedAsync(StoreContext dbcontext) 
        {
            // must  add object of class that not depend on another

            // to execute if not have any data
            if (!dbcontext.ProductBrand.Any())
            {
                #region Add DataSeed of ProductBrand
                var brandaData = File.ReadAllText("../Talabat.RepositoryLayer/Data/DataSeeding/brands.json"); //Read data as string (serialize)
                                                                                                              // convert string to object of type brand to can store in DB
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandaData);

                if (/*brands is not null && brands.Count>0*/brands?.Count > 0)
                {
                    foreach (var brand in brands)
                    {
                        await dbcontext.Set<ProductBrand>().AddAsync(brand);
                    }
                }
                await dbcontext.SaveChangesAsync();
                #endregion
            }

            if (!dbcontext.ProductType.Any())
            {
                #region Add DataSeed of ProductType

                var DataProductType = File.ReadAllText("../Talabat.RepositoryLayer/Data/DataSeeding/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(DataProductType);

                if (types?.Count > 0)
                {

                    foreach (var item in types)
                    {
                        await dbcontext.Set<ProductType>().AddAsync(item);
                    }
                    await dbcontext.SaveChangesAsync();
                }

                #endregion
            }

            if (!dbcontext.Products.Any())
            {
                #region Add DataSeed of Product

                var DataProduct = File.ReadAllText("../Talabat.RepositoryLayer/Data/DataSeeding/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(DataProduct);

                if (products?.Count > 0)
                {

                    foreach (var item in products)
                    {
                        await dbcontext.Set<Product>().AddAsync(item);
                    }
                    await dbcontext.SaveChangesAsync();
                }

                #endregion
            }

            


        }

    }
}
