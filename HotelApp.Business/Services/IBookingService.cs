using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Data.Models;

namespace HotelApp.Business.Services;

public interface IBookingService : IBaseService<Booking>
{
    new Task<List<BookingViewModel>> GetAllAsync();

    new Task<BookingViewModel> GetByIdAsync(Guid id);

    Task<BookingViewModel> BookingRoomAsync(BookingRoomRequest request);

    Task<BookingAmenityViewModel> BookingAmenityAsync(BookingAmenityRequest request);

    Task<BookingViewModel> CheckInAsync(CheckInRequest request);

    Task<CheckOutViewModel> CheckOutAsync(CheckOutRequest request);
}
