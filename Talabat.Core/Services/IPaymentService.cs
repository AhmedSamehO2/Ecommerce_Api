using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);

        Task<Order> UpdatePaymentIntentSuccssedOrFailed(string paymentIntent, bool flag);
    }
}
