using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Business.ViewModels.Room;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Business.Services;

public class BookingService(IUnitOfWork unitOfWork) :
    BaseService<Booking>(unitOfWork), IBookingService
{
    public new async Task<List<BookingViewModel>> GetAllAsync()
    {
        var bookings = await _unitOfWork.BookingRepository.GetQuery()
            .Include(b => b.Customer)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .ToListAsync();

        // Map Booking to BookingViewModel
        var bookingViewModels = bookings.Select(booking => new BookingViewModel
        {
            Id = booking.Id,
            CustomerId = booking.CustomerId,
            BookingDate = booking.BookingDate.DateTime,
            CheckInDate = booking.CheckInDate.DateTime,
            CheckOutDate = booking.CheckOutDate.DateTime,
            Rooms = booking.BookingDetails.Select(bd => new RoomViewModel
            {
                Id = bd.Room!.Id,
                Number = bd.Room.Number,
                Type = bd.Room.Type,
                Capacity = bd.Room.Capacity,
                PricePerNight = bd.Price,
            }).ToList() ?? []
        }).ToList();

        return bookingViewModels;
    }

    public async new Task<BookingViewModel> GetByIdAsync(Guid id)
    {
        var booking = await _unitOfWork.BookingRepository.GetQuery()
            .Include(b => b.Customer)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            throw new ArgumentException($"Booking with id {id} not found");
        }

        // Map Booking to BookingViewModel
        var bookingViewModel = new BookingViewModel
        {
            Id = booking.Id,
            CustomerId = booking.CustomerId,
            BookingDate = booking.BookingDate.DateTime,
            CheckInDate = booking.CheckInDate.DateTime,
            CheckOutDate = booking.CheckOutDate.DateTime,
            Rooms = booking.BookingDetails.Select(bd => new RoomViewModel
            {
                Id = bd.Room!.Id,
                Number = bd.Room.Number,
                Type = bd.Room.Type,
                Capacity = bd.Room.Capacity,
                PricePerNight = bd.Price,
            }).ToList() ?? []
        };

        return bookingViewModel;
    }
}

