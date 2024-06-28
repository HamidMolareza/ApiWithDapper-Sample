namespace ApiWithDapper.Helpers;

public class PageData<T> {
    public int Page { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int? Limit { get; set; }
    public List<T> Data { get; set; } = [];
}