using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.Amenity;

public class AmenityCreateUpdateViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, MinimumLength = 2, ErrorMessage = "Name must be between {2} and {1} characters")]
    public required string Name { get; set; }

    [StringLength(1000, ErrorMessage = "Description must be less than {1} characters")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to {1}")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "IsActive is required")]
    public bool IsActive { get; set; }
}