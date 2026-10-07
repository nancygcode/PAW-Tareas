namespace PAW.Web.Models
{
    public interface IPagedResult
    {
        int Page { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPages { get; }
    }

    /// <summary>A single page of items (max 25 per page by default).</summary>
    public class PagedResult<T> : IPagedResult
    {
        public const int MaxPageSize = 25;

        public IReadOnlyList<T> Items { get; init; } = [];
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = MaxPageSize;
        public int TotalItems { get; init; }
        public int TotalPages => Math.Max((int)Math.Ceiling(TotalItems / (double)PageSize), 1);

        public static PagedResult<T> Create(IEnumerable<T> source, int page, int pageSize = MaxPageSize)
        {
            var list = source.ToList();
            pageSize = Math.Clamp(pageSize, 1, MaxPageSize);

            var totalPages = Math.Max((int)Math.Ceiling(list.Count / (double)pageSize), 1);
            page = Math.Clamp(page, 1, totalPages);

            return new PagedResult<T>
            {
                Items = list.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalItems = list.Count
            };
        }
    }
}
