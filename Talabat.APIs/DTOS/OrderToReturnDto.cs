using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.APIs.DTOS
{
    public class OrderToReturnDto
    {
        public string Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } // Date
        public string Status { get; set; } 
        public OrderAddress ShappingAddress { get; set; }
        public string DeliveryMethod { get; set; } // Name
        public decimal DeliveryMethodCost { get; set; } // Cost
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; } // Price Of Product * Quantity
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } 
    }
}
