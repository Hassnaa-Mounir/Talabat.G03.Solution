using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Repositories;
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
            return await dbcontext.Set<T>().ToListAsync();   
        }

        public async Task<T> GetByIdAsync(int id)
        {
            //return await dbcontext.Set<T>().Where(x => x.Id == id).FirstOrDefaultAsync();
            return await dbcontext.Set<T>().FindAsync(id); // search localy first
        }
    }
}
