using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Role;
using HotelApp.Data.Models;

namespace HotelApp.Business.Services;

public interface IRoleService : IBaseService<Role>
{
    /// <summary>
    /// Creates a new role asynchronously.
    /// </summary>
    /// <param name="request">The view model containing the role details.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the role was created successfully.</returns>
    Task<bool> AddAsync(RoleCreateUpdateViewModel request);

    /// <summary>
    /// Updates an existing role asynchronously.
    /// </summary>
    /// <param name="id">The ID of the role to update.</param>
    /// <param name="request">The view model containing the updated role details.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the role was updated successfully.</returns>
    Task<bool> UpdateAsync(Guid id, RoleCreateUpdateViewModel request);
}
