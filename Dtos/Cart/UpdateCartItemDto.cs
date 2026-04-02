using System.ComponentModel.DataAnnotations;

namespace Mini_E_Commerce.Dtos.Cart
{
    public class UpdateCartItemDto
    {
        [Required]
        public int CartItemId { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
