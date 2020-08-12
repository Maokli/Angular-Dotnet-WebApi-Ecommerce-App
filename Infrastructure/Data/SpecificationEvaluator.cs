using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity:BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
         ISpecification<TEntity> spec){
             var query = inputQuery;
             query = spec.Includes.Aggregate(query , 
             (current , include) => current.Include(include));
             if(spec.Criteria != null)           
                return query.Where(spec.Criteria);
             
             return query;
        }
    }
}