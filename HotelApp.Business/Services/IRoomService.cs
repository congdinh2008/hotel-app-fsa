using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Room;
using HotelApp.Data.Models;

namespace HotelApp.Business.Services;

public interface IRoomService: IBaseService<Room>
{
    new Task<List<RoomViewModel>> GetAllAsync();

    new Task<RoomViewModel> GetByIdAsync(Guid id);

    Task<int> AddAsync(RoomCreateUpdateViewModel roomCreateViewModel);

    Task<bool> UpdateAsync(Guid id, RoomCreateUpdateViewModel roomUpdateViewModel);
}
