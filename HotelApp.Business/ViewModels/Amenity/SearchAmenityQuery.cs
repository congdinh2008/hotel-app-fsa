namespace HotelApp.Business.ViewModels.Amenity
{
    public class SearchAmenityQuery : SearchQuery
    {
        public string? Name { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}