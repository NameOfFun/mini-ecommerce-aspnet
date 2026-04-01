namespace Mini_E_Commerce.Dtos.Common
{
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Data { get; set; } = [];

        public int Page { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}
