namespace HotelApp.Business.ViewModels;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PaginatedResult<T>(List<T> items, int count, int pageIndex, int pageSize)
{
    public PageInfo PageInfo { get; set; } = new PageInfo(count, pageIndex, pageSize);

    public T[] Items { get; set; } = items.ToArray();

    public static async Task<PaginatedResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize)
    {
        var count = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedResult<T>(items, count, pageIndex, pageSize);
    }
}

public class PageInfo(int count, int pageIndex, int pageSize)
{
    public int PageIndex { get; private set; } = pageIndex;

    public int PageSize { get; set; } = pageSize;

    public int TotalItems { get; set; } = count;

    public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);
}