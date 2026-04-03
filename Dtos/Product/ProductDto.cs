namespace Mini_E_Commerce.Dtos.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public string? ProductAlias { get; set; }

        public int CategoryId { get; set; }

        public string? UnitDescription { get; set; }

        public double? UnitPrice { get; set; }

        public string? Image { get; set; }

        public DateTime ManufactureDate { get; set; }

        public double Discount { get; set; }

        public int ViewCount { get; set; }

        public string? Description { get; set; }

        public string SupplierId { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public int Stock { get; set; }
    }
}
