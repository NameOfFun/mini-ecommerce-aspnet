namespace Mini_E_Commerce.Dtos.Cart
{
    public class CartResponseDto
    {
        public int CartId { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = [];
        public double TotalAmount => Items.Sum(i => i.Subtotal);
        public int TotalItems => Items.Sum(i => i.Quantity);
        public int TotalProducts => Items.Count;
        public bool IsEmpty => !Items.Any();
    }
    public class CartItemResponseDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Subtotal => Quantity * UnitPrice;
        public int StockAvailable { get; set; } // Lấy số lượng tồn kho hiện tại của sản phẩm
        public bool IsEnoughStock => StockAvailable >= Quantity;
    }
}
