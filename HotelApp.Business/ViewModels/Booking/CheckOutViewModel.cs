using HotelApp.Business.ViewModels.Room;

namespace HotelApp.Business.ViewModels.Booking;

public class CheckOutViewModel
{
    public Guid BookingId { get; set; }

    public required string CustomerName { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public ICollection<RoomViewModel> Rooms { get; set; } = [];

    public decimal Cost { get; set; }
}
