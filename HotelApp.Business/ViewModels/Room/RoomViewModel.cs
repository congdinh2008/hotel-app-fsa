using HotelApp.Data.Enums;

namespace HotelApp.Business.ViewModels.Room;

public class RoomViewModel
{
    public Guid Id { get; set; }
    
    public required string Number { get; set; }
    
    public RoomType Type { get; set; }
    
    public int Capacity { get; set; }
    
    public decimal PricePerNight { get; set; }

    public RoomStatus Status { get; set; }

    public bool IsActive { get; set; }
}
