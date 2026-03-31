namespace Mini_E_Commerce.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        
        public string UserId { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public virtual AppUser User { get; set; } = null!;

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
