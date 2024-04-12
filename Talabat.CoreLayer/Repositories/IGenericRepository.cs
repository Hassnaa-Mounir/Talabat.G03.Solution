using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;

namespace Talabat.CoreLayer.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel // to  can sepecific classes will accept only class basemodel or any class inhert from it
    {
     //GetAll Data   
       Task<IEnumerable<T>> GetAllAsync();

     //Get by Id
       Task<T> GetByIdAsync(int id);

    }
}
