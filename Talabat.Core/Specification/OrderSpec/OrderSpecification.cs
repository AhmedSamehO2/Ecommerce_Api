using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.Core.Specification.OrderSpec
{
    public class OrderSpecification : BaseSpecifications<Order>
    {
        public OrderSpecification(string email):base(o=>o.BuyerEmail==email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDesc(O => O.OrderDate);

        }

        public OrderSpecification(string email , int OrderId):base(Object=>Object.BuyerEmail == email && Object.Id == OrderId) 
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
