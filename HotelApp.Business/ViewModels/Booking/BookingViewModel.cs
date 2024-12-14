using HotelApp.Business.ViewModels.Room;

namespace HotelApp.Business.ViewModels.Booking;

public class BookingViewModel
{
    public Guid Id { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime CheckInDate { get; set; }

    public DateTime CheckOutDate { get; set; }

    public Guid CustomerId { get; set; }

    public ICollection<RoomViewModel> Rooms { get; set; } = [];
}
