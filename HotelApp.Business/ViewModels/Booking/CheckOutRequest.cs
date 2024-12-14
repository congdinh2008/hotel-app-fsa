using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.Booking;

public class CheckOutRequest
{
    [Required(ErrorMessage = "Booking Id is required")]
    public Guid BookingId { get; set; }

    [Required(ErrorMessage = "Customer Id is required")]
    public Guid CustomerId { get; set; }

    [Required(ErrorMessage = "Check Out Date is required")]
    public DateTime CheckOutDate { get; set; }
}
