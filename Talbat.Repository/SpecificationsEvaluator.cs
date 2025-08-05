using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;
using Talbat.Core.Specifications;

namespace Talbat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        // First Parameter is DbSet ,Second Parameter is Specifications 
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery , ISpecifications<TEntity> spec)
        {
            var query = inputQuery;// _dbContext.Set<Product>()
            if (spec.Criteria is not null) // P => P.Id == 1 
                query = query.Where(spec.Criteria);

            if(spec.OrderBy is not null) // P => P.Name
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc is not null) // P => P.Price
                query = query.OrderByDescending(spec.OrderByDesc);

            // query = _dbContext.Set<Product>().Where(P=>P.Id==1);

            // Includes 
            //1. P=>P.Brand
            //2. P=>P.Category

            if (spec.IsPagenationEnabled) // If Pagination is Enabled
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Include.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            // _products.where().OrderBy().Skip().Take().Include(p => p.Brand).Include(p => p.Category);
            return query;
        }
    }
}
