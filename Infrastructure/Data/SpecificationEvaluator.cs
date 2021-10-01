using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            IQueryable<TEntity> input = inputQuery;
            if (specification.Criteria != null)
                input = input.Where(specification.Criteria);

            if (specification.OrderBy != null)
                input = input.OrderBy(specification.OrderBy);

            if (specification.OrderByDescending != null)
                input = input.OrderByDescending(specification.OrderByDescending);

            // pagination always last
            if (specification.IsPagingEnabled)
                input = input.Skip(specification.Skip).Take(specification.Take);

            input = specification.Includes.Aggregate(input, (current, include) => current.Include(include));

            return input;
        }
    }
}
