using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.Booking;

public class BookingAmenityRequest
{
    [Required(ErrorMessage = "Booking Id is required")]
    public Guid BookingId { get; set; }
    
    [Required(ErrorMessage = "Amenity Id is required")]
    public Guid AmenityId { get; set; }
    
    [Required(ErrorMessage = "Room Id is required")]
    public Guid RoomId { get; set; }
    
    [Required(ErrorMessage = "Booking Date is required")]
    public DateTime BookingDate { get; set; }
    
    [Required(ErrorMessage = "Quantity is required")]
    public int Quantity { get; set; }
    
    [Required(ErrorMessage = "Price is required")]
    public decimal Price { get; set; }
}
