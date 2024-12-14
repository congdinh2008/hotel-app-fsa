namespace HotelApp.Business.ViewModels.Amenity;

public class AmenityViewModel
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
