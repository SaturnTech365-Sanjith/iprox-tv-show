namespace Iprox.Application.Common.Dtos;

public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Items { get; set; }

    public PagedResult(int totalCount, IEnumerable<T> items, int pageNumber, int pageSize)
    {
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        CurrentPage = pageNumber;
        PageSize = pageSize;
        Items = items;
    }
}
