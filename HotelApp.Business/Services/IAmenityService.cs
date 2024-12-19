using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels;
using HotelApp.Business.ViewModels.Amenity;
using HotelApp.Data.Models;

namespace HotelApp.Business.Services;

public interface IAmenityService : IBaseService<Amenity>
{
    new Task<List<AmenityViewModel>> GetAllAsync();

    new Task<AmenityViewModel> GetByIdAsync(Guid id);

    Task<PaginatedResult<AmenityViewModel>> SearchAsync(SearchAmenityQuery request);

    Task<int> AddAsync(AmenityCreateUpdateViewModel amenityCreateViewModel);

    Task<bool> UpdateAsync(Guid id, AmenityCreateUpdateViewModel amenityUpdateViewModel);
}
