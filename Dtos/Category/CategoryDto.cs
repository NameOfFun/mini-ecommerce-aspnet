namespace Mini_E_Commerce.Dtos.Category
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? CategoryAlias { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }
    }
}