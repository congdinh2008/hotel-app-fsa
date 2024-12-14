using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Room;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Business.Services;

public class RoomService(IUnitOfWork unitOfWork) :
    BaseService<Room>(unitOfWork), IRoomService
{
    public Task<int> AddAsync(RoomCreateUpdateViewModel roomCreateViewModel)
    {
        // Check if room is null
        if (roomCreateViewModel == null)
        {
            throw new ArgumentNullException(nameof(roomCreateViewModel), "Room is null");
        }

        // Check if room number is already taken
        if (_unitOfWork.RoomRepository.GetQuery().Any(r => r.Number == roomCreateViewModel.Number))
        {
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
            throw new ArgumentNullException(nameof(roomUpdateViewModel), "Room is null");
        }

        // Get room by id
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);

        // Check if room exists
        if (room == null)
        {
            throw new ArgumentException($"Room with id {id} not found");
        }

        // Check if room number is already taken
        if (await _unitOfWork.RoomRepository.GetQuery().AnyAsync(r => r.Number == roomUpdateViewModel.Number && r.Id != id))
        {
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
}
