using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Specifications;

namespace Talabat.CoreLayer.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel // to  can sepecific classes will accept only class basemodel or any class inhert from it
    {
     //GetAll Data   
       Task<IReadOnlyList<T>> GetAllAsync();

     //Get by Id
       Task<T> GetByIdAsync(int id);
        Task<T?> GetWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
