namespace HotelApp.Business.ViewModels.Room;

public class SearchRoomQuery : SearchQuery
{
    public string? Number { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }
}