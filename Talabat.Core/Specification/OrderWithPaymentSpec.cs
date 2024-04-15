using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.Core.Specification
{
    public class OrderWithPaymentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentSpec(string PaymentIntentId):base(O=>O.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
