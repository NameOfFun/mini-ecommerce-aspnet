using System.ComponentModel.DataAnnotations;

namespace Mini_E_Commerce.Dtos.Cart
{
    public class AddToCartDto
    {
        [Required]
        public int ProductId { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
    }
}
