namespace HotelApp.Business.ViewModels;

using Microsoft.EntityFrameworkCore;

public class PaginatedResult<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public int PageIndex { get; private set; } = pageIndex;
    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
    public T[] Items { get; set; } = [.. items];

    public bool HasPreviousPage
    {
        get
        {
            return PageIndex > 1;
        }
    }

    public bool HasNextPage
    {
        get
        {
            return PageIndex < TotalPages;
        }
    }

    public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        var count = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResult<T>(items, count, pageIndex, pageSize);
    }
}
