using Talabat.Core.Entities.OrderAggregat;

namespace Talabat.APIs.DTOS
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int Qunatity { get; set; }
    }
}