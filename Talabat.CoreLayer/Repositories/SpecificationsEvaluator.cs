using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.CoreLayer.Entities;
using Talabat.CoreLayer.Specifications;

namespace Talabat.CoreLayer.Repositories
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseModel
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec) 
        {
            var query = inputQuery;

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpressiom) => currentQuery.Include(includeExpressiom));

            return query;
        }

        public IQueryable<T> GetQuery<T>(object value, ISpecification<T> spec) where T : BaseModel
        {
            throw new NotImplementedException();
        }
    }
}
