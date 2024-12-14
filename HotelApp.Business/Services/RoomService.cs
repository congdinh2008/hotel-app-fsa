using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Room;
using HotelApp.Data.Enums;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class RoomService(IUnitOfWork unitOfWork, ILogger<RoomService> logger) :
    BaseService<Room>(unitOfWork, logger), IRoomService
{
    public Task<int> AddAsync(RoomCreateUpdateViewModel roomCreateViewModel)
    {
        // Check if room is null
        if (roomCreateViewModel == null)
        {
            _logger.LogError("Room is null");
            throw new ArgumentNullException(nameof(roomCreateViewModel), "Room is null");
        }

        // Check if room number is already taken
        if (_unitOfWork.RoomRepository.GetQuery().Any(r => r.Number == roomCreateViewModel.Number))
        {
            _logger.LogError("Room with number {RoomNumber} already exists", roomCreateViewModel.Number);
            throw new ArgumentException($"Room with number {roomCreateViewModel.Number} already exists");
        }

        // Create new room
        var room = new Room
        {
            Number = roomCreateViewModel.Number,
            Type = roomCreateViewModel.Type,
            Capacity = roomCreateViewModel.Capacity,
            PricePerNight = roomCreateViewModel.PricePerNight,
            Status = roomCreateViewModel.Status,
            IsActive = roomCreateViewModel.IsActive
        };

        // Add room to database
        _unitOfWork.RoomRepository.Add(room);
        return _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, RoomCreateUpdateViewModel roomUpdateViewModel)
    {
        // Check if room is null
        if (roomUpdateViewModel == null)
        {
            _logger.LogError("Room is null");
            throw new ArgumentNullException(nameof(roomUpdateViewModel), "Room is null");
        }

        // Get room by id
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

        // Check if room exists
        if (room == null)
        {
            _logger.LogError("Room with id {RoomId} not found", id);
            throw new ArgumentException($"Room with id {id} not found");
        }

        // Check if room number is already taken
        if (await _unitOfWork.RoomRepository.GetQuery().AnyAsync(r => r.Number == roomUpdateViewModel.Number && r.Id != id))
        {
            _logger.LogError("Room with number {RoomNumber} already exists", roomUpdateViewModel.Number);
            throw new ArgumentException($"Room with number {roomUpdateViewModel.Number} already exists");
        }

        // Update room
        room.Number = roomUpdateViewModel.Number;
        room.Type = roomUpdateViewModel.Type;
        room.Capacity = roomUpdateViewModel.Capacity;
        room.PricePerNight = roomUpdateViewModel.PricePerNight;
        room.Status = roomUpdateViewModel.Status;
        room.IsActive = roomUpdateViewModel.IsActive;

        // Update room in database
        _unitOfWork.RoomRepository.Update(room);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async new Task<List<RoomViewModel>> GetAllAsync()
    {
        var rooms = await base.GetAllAsync();

        // Map Room to RoomViewModel
        var roomViewModels = rooms.Select(room => new RoomViewModel
        {
            Id = room.Id,
            Number = room.Number,
            Type = room.Type,
            Capacity = room.Capacity,
            PricePerNight = room.PricePerNight,
            Status = room.Status,
            IsActive = room.IsActive
        }).ToList();

        return roomViewModels;
    }

    public async new Task<RoomViewModel> GetByIdAsync(Guid id)
    {
        var room = await base.GetByIdAsync(id);

        if (room == null)
        {
            _logger.LogError("Room with id {RoomId} not found", id);
            throw new ArgumentException($"Room with id {id} not found");
        }

        // Map Room to RoomViewModel
        var roomViewModel = new RoomViewModel
        {
            Id = room.Id,
            Number = room.Number,
            Type = room.Type,
            Capacity = room.Capacity,
            PricePerNight = room.PricePerNight,
            Status = room.Status,
            IsActive = room.IsActive
        };

        return roomViewModel;
    }

    public async Task<List<RoomViewModel>> SearchAvailableRoomAsync(SearchingAvailableRoomRequest request)
    {
        if (request.CheckInDate >= request.CheckOutDate)
        {
            _logger.LogError("Check-out Date must be greater than Check-in Date");
            throw new ArgumentException("Check-out Date must be greater than Check-in Date");
        }

        var availableRooms = await _unitOfWork.RoomRepository.GetQuery()
            .Where(r => r.Status == RoomStatus.Available &&
                _unitOfWork.Context.Bookings.Any(b => b.BookingDetails.Any(
                    bd => bd.RoomId == r.Id &&
                    b.CheckInDate < request.CheckOutDate &&
                    b.CheckOutDate > request.CheckInDate)))
            .Select(r => new RoomViewModel
            {
                Id = r.Id,
                Number = r.Number,
                Type = r.Type,
                Capacity = r.Capacity,
                PricePerNight = r.PricePerNight,
                Status = r.Status,
                IsActive = r.IsActive
            })
            .ToListAsync();

        return availableRooms ?? [];
    }
}
