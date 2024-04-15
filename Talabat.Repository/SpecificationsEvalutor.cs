using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    public static class SpecificationsEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery , ISpecification<T> Spec)
        {
            var query = InputQuery;
            if(Spec.Criteria is not null)
            {
                query = query.Where(Spec.Criteria);
            }
            if(Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            if(Spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(Spec.OrderByDesc);
            }
            if(Spec.IsPAginationEnable)
            {
                query = query.Skip(Spec.Sikp).Take(Spec.Take);
            }
            query = Spec.Includes.Aggregate(query, (CurrentQuery, IncludesExpression) => CurrentQuery.Include(IncludesExpression));
            return query;
        }
    }
}
