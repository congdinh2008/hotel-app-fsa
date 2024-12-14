using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Business.ViewModels.Room;
using HotelApp.Data.Enums;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class BookingService(IUnitOfWork unitOfWork) :
    BaseService<Booking>(unitOfWork), IBookingService
{
    public async Task<BookingViewModel> BookingRoomAsync(BookingRoomRequest request)
    {
        // Check if booking date is valid
        if (request.BookingDate >= request.CheckInDate)
        {
            throw new ArgumentException("Check-in Date must be greater than Booking Date");
        }

        // Check if customer is valid
        var customer = await _unitOfWork.UserRepository.GetByIdAsync(request.CustomerId) ?? throw new ArgumentException("Customer not found");

        var roomIds = request.Rooms.Select(rb => rb.RoomId).ToList();

        // Check if rooms are valid
        var rooms = await _unitOfWork.RoomRepository.GetQuery()
            .Where(r => roomIds.Contains(r.Id))
            .ToListAsync();

        if (rooms.Count != request.Rooms.Count)
        {
            throw new ArgumentException("One or more rooms not found");
        }

        // Check if rooms are available
        var unavailableRooms = await (from room in _unitOfWork.RoomRepository.GetQuery()
                                      join bookingDetail in _unitOfWork.Context.BookingDetails on room.Id equals bookingDetail.RoomId
                                      join bookingQuery in _unitOfWork.Context.Bookings on bookingDetail.BookingId equals bookingQuery.Id
                                      where room.Status != RoomStatus.Available
                                      where request.CheckInDate < bookingQuery.CheckOutDate
                                      where request.CheckOutDate > bookingQuery.CheckInDate
                                      where roomIds.Contains(room.Id)
                                      select room).ToListAsync();

        if (unavailableRooms.Any())
        {
            throw new ArgumentException("One or more rooms are not available");
        }

        // Create booking
        var booking = new Booking
        {
            Customer = customer,
            BookingDate = request.BookingDate,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Status = BookingStatus.Pending
        };

        // Add rooms to booking
        foreach (var room in rooms)
        {
            var bookingDetail = new BookingDetail
            {
                Booking = booking,
                Room = room,
                Price = room.PricePerNight
            };

            booking.BookingDetails.Add(bookingDetail);
        }

        _unitOfWork.BookingRepository.Add(booking);

        await _unitOfWork.SaveChangesAsync();

        return new BookingViewModel
        {
            Id = booking.Id,
            CustomerId = booking.Customer.Id,
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
            }).ToList()
        };
    }

    public async Task<BookingAmenityViewModel> BookingAmenityAsync(BookingAmenityRequest request)
    {
        // Check if booking is valid
        var booking = await _unitOfWork.BookingRepository.GetByIdAsync(request.BookingId);

        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        // Check if amenity is valid
        var amenity = await _unitOfWork.AmenityRepository.GetByIdAsync(request.AmenityId);

        if (amenity == null)
        {
            throw new ArgumentException("Amenity not found");
        }

        // Check if room is valid
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(request.RoomId);

        if (room == null)
        {
            throw new ArgumentException("Room not found");
        }

        // Create booking amenity
        var bookingAmenity = new BookingAmenity
        {
            Booking = booking,
            Amenity = amenity,
            Room = room,
            BookingDate = request.BookingDate,
            Quantity = request.Quantity,
            Price = request.Price
        };

        _unitOfWork.BookingAmenityRepository.Add(bookingAmenity);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new InvalidOperationException("Booking Amenity not added");
        }

        return new BookingAmenityViewModel
        {
            BookingId = bookingAmenity.BookingId,
            CustomerName = bookingAmenity.Booking.Customer.DisplayName,
            AmenityName = bookingAmenity.Amenity.Name,
            BookingDate = bookingAmenity.BookingDate.DateTime,
            Quantity = bookingAmenity.Quantity,
            Price = bookingAmenity.Price,
            Room = new RoomViewModel
            {
                Id = bookingAmenity.Room.Id,
                Number = bookingAmenity.Room.Number,
                Type = bookingAmenity.Room.Type,
                Capacity = bookingAmenity.Room.Capacity,
                PricePerNight = bookingAmenity.Room.PricePerNight
            }
        };

    }

    public async Task<BookingViewModel> CheckInAsync(CheckInRequest request)
    {
        // Check if booking is valid
        var booking = await _unitOfWork.BookingRepository.GetQuery()
            .Include(r => r.Customer)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId);

        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        // Check if customer is valid
        var customer = await _unitOfWork.UserRepository.GetByIdAsync(request.CustomerId);

        if (customer == null)
        {
            throw new ArgumentException("Customer not found");
        }

        // Check check-in date
        if (request.CheckInDate < booking.CheckInDate)
        {
            throw new ArgumentException("Check-in Date must be greater than or equal to Booking Check-in Date");
        }

        // Check check-out date
        if (request.CheckInDate >= booking.CheckOutDate)
        {
            throw new ArgumentException("Check-in Date must be less than Booking Check-out Date");
        }

        // Update booking status
        booking.Status = BookingStatus.CheckedIn;
        booking.CheckInDate = request.CheckInDate;

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new InvalidOperationException("Booking cannot be checked in or already checked in");
        }

        return new BookingViewModel
        {
            Id = booking.Id,
            CustomerId = customer.Id,
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
            }).ToList()
        };
    }

    public async Task<CheckOutViewModel> CheckOutAsync(CheckOutRequest request)
    {
        // Check if booking is valid
        var booking = await _unitOfWork.BookingRepository.GetQuery()
            .Include(r => r.Customer)
            .Include(b => b.BookingDetails)
            .ThenInclude(bd => bd.Room)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId);

        if (booking == null)
        {
            throw new ArgumentException("Booking not found");
        }

        // Check if customer is valid
        var customer = await _unitOfWork.UserRepository.GetByIdAsync(request.CustomerId);

        if (customer == null)
        {
            throw new ArgumentException("Customer not found");
        }

        // Calculate total price
        var roomPrice = booking.BookingDetails.Sum(bd => bd.Price);

        var bookingServicesPrice = await _unitOfWork.BookingAmenityRepository.GetQuery()
            .Where(bs => bs.BookingId == booking.Id)
            .SumAsync(bs => bs.Price);

        var totalPrice = roomPrice + bookingServicesPrice;

        if (totalPrice <= 0)
        {
            throw new InvalidOperationException("Total price cannot be zero or negative");
        }

        // Update booking status
        booking.Status = BookingStatus.CheckedOut;
        booking.CheckOutDate = request.CheckOutDate;

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new InvalidOperationException("Booking cannot be checked out or already checked out");
        }

        return new CheckOutViewModel
        {
            BookingId = booking.Id,
            CustomerName = customer.DisplayName,
            BookingDate = booking.BookingDate.DateTime,
            CheckInDate = booking.CheckInDate.DateTime,
            CheckOutDate = request.CheckOutDate,
            Cost = totalPrice,
            Rooms = booking.BookingDetails.Select(bd => new RoomViewModel
            {
                Id = bd.Room!.Id,
                Number = bd.Room.Number,
                Type = bd.Room.Type,
                Capacity = bd.Room.Capacity,
                PricePerNight = bd.Price,
            }).ToList()
        };
    }

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

