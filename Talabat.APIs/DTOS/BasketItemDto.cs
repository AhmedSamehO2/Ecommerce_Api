using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOS
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
       
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Quantity must be At Least one Product")]
        public int Quantity { get; set; }
    }
}
