using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.Room;

public class SearchingAvailableRoomRequest
{
    [Required(ErrorMessage = "CheckInDate is required")]
    public DateTime CheckInDate { get; set; }

    [Required(ErrorMessage = "CheckOutDate is required")]
    public DateTime CheckOutDate { get; set; }
}
