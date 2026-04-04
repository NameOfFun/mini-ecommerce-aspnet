using System.ComponentModel.DataAnnotations;

namespace Mini_E_Commerce.Dtos.Order
{
    public class CheckoutDto
    {
        [Required, MaxLength(100)]
        public string Address { get; set; } = null!;
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;
        [MaxLength(100)]
        public string PaymentMethod { get; set; } = "Cash";
        [MaxLength(100)]
        public string ShippingMethod { get; set; } = "Standard";
        public double ShippingFee { get; set; }
        [MaxLength(100)]
        public string? Note { get; set; }
    }
}
