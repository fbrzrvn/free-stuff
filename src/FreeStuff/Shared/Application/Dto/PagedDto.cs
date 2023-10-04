namespace FreeStuff.Shared.Application.Dto;

public class PagedDto<T>
{
    public IEnumerable<T> Data         { get; init; } = Enumerable.Empty<T>();
    public int?           Page         { get; init; }
    public int            Limit        { get; init; }
    public int            TotalResults { get; init; }
    public bool           HasNextPage  => TotalResults > Page * Limit;
}
