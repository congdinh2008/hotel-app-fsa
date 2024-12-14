using HotelApp.Business.ViewModels.Room;

namespace HotelApp.Business.ViewModels.Booking;

public class BookingAmenityViewModel
{
    public Guid BookingId { get; set; }

    public required string CustomerName { get; set; }

    public required string AmenityName { get; set; }

    public DateTime BookingDate { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public RoomViewModel? Room { get; set; }
}
