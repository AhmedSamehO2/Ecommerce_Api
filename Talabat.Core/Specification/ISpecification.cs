﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        // _dbContext.products.Where(P => P.id == Id).Include(P=>P.ProductBrand).Include(P=>P.ProductType)
        public Expression<Func<T,bool>> Criteria { get; set; }
        public List <Expression<Func<T,object>>> Includes { get; set; }
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Sikp { get; set; }
        public bool IsPAginationEnable { get; set; }


    }
}
