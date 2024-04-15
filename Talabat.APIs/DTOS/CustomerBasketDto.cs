using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOS
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? clientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        public List<BasketItemDto> Items { get; set; }
    }
}
