namespace Mini_E_Commerce.Dtos.Order
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string StatusName { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public double ShippingFee { get; set; }
        public double ItemsTotal { get; set; }
        public double GrandTotal { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = [];
    }

    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
    }
}
