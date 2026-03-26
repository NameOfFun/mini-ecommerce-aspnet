namespace Mini_E_Commerce.Dtos.Product
{
    public class ProductQueryDto
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int Page { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string? Search { get; set; }
        public  int? CategoryId { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
