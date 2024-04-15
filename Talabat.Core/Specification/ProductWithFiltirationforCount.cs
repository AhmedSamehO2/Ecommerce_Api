using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithFiltirationforCount : BaseSpecifications<Product>
    {
        public ProductWithFiltirationforCount(ProductSpecParams Params) : base
            (P =>
           (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.brandId.HasValue || P.ProductBrandId == Params.brandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {

        }
    }
}
