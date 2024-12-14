using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.Booking;

public class BookingRoomRequest
{
    [Required(ErrorMessage = "Customer Id is required")]
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "Booking Date is required")]   
    public DateTime BookingDate { get; set; }

    [Required(ErrorMessage = "Check In Date is required")]
    public DateTime CheckInDate { get; set; }
    
    [Required(ErrorMessage = "Check Out Date is required")]
    public DateTime CheckOutDate { get; set; }

    [Required(ErrorMessage = "Room Ids are required")]
    public ICollection<RoomBookingViewModel> Rooms { get; set; } = [];

}
