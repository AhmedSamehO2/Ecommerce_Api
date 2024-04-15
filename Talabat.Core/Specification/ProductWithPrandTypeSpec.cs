using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithPrandTypeSpec : BaseSpecifications<Product>
    {
        public ProductWithPrandTypeSpec(ProductSpecParams Params) : base
            (P=>
            (string.IsNullOrEmpty(Params.Search)||P.Name.ToLower().Contains(Params.Search))
            &&
            (!Params.brandId.HasValue ||P.ProductBrandId== Params.brandId)
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            ) 
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                        default:
                        AddOrderBy(P => P.Name);
                        break;
                            

                }
            }

            // Products = 100 , PageSize = 10 PageIndex = 5;
            ApplyPAgination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        public ProductWithPrandTypeSpec(int id) : base(P=>P.Id == id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
           
        }
    }
}
