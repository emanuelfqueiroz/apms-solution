
namespace AffiliatePMS.Application.Common
{
    public record Pagination
    {
        public int PageSize { get; set; } = 10;
        public string? ContinuationToken { get; set; }
    }

    public record class PaginationResponse<T>
    {
        public Pagination Page { get; set; }
        public T? Data { get; set; }
    }
    public record class PaginationTrackResponse<T> : PaginationResponse<T>
    {
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
