namespace Mini_E_Commerce.Dtos.Supplier
{
    public class SupplierDto
    {
        public string SupplierId { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string Logo { get; set; } = null!;

        public string? ContactPerson { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
    }
}
