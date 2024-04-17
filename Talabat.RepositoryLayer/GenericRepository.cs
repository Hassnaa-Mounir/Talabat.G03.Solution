using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
using Talabat.CoreLayer.Specifications;
using Talabat.RepositoryLayer.Data;

namespace Talabat.RepositoryLayer
{
    // not necessery make repository class for each entity buz all same not have diff structure 
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext dbcontext;

        public GenericRepository(StoreContext dbcontext) //Ask CLR that Assign object from dbcontext
        {
            this.dbcontext = dbcontext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            { 
             return (IEnumerable<T>) await dbcontext.Products.Include(p => p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            }

            return await dbcontext.Set<T>().ToListAsync();   
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T?> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }
    }
}
