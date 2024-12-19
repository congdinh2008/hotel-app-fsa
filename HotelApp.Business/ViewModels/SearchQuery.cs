namespace HotelApp.Business.ViewModels;

public class SearchQuery
{
    public int Page { get; set; }

    public int Size { get; set; }

    public string? OrderBy { get; set; }

    public OrderDirection OrderDirection { get; set; }
}

public enum OrderDirection
{
    ASC,
    DESC
}