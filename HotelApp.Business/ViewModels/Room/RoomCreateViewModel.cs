using System.ComponentModel.DataAnnotations;
using HotelApp.Data.Enums;

namespace HotelApp.Business.ViewModels.Room;

public class RoomCreateUpdateViewModel
{
    [Required(ErrorMessage = "Number is required")]
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Number must be between {2} and {1} characters")]
    public required string Number { get; set; }
    
    [Required(ErrorMessage = "Type is required")]
    public RoomType Type { get; set; }
    
    [Required(ErrorMessage = "Capacity is required")]
    [Range(1, 10, ErrorMessage = "Capacity must be between {1} and {2}")]
    public int Capacity { get; set; }
    
    [Required(ErrorMessage = "Price per night is required")]
    [Range(1, 10000, ErrorMessage = "Price per night must be between {1} and {2}")]
    public decimal PricePerNight { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public RoomStatus Status { get; set; }

    [Required(ErrorMessage = "Active is required")]
    public bool IsActive { get; set; }
}
