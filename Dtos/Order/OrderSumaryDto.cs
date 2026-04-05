namespace Mini_E_Commerce.Dtos.Order
{
    public class OrderSumaryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusName { get; set; } = null!;
        public double GrandTotal { get; set; }
        public string? UserEmail { get; set; } // chỉ Admin cần - map từ User
    }
}
