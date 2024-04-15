using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail,string BasketId,int DeliveryMethodId, OrderAddress address);

        Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string BuyerEmail);
        Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail , int OrderId);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
