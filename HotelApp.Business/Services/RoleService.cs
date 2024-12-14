using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.Role;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class RoleService(IUnitOfWork unitOfWork, ILogger<RoleService> logger, RoleManager<Role> roleManager) :
    BaseService<Role>(unitOfWork, logger), IRoleService
{
    private readonly RoleManager<Role> _roleManager = roleManager;

    /// <summary>
    /// Creates a new role asynchronously.
    /// </summary>
    /// <param name="request">The view model containing role creation data.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the role was created successfully.</returns>
    public async Task<bool> AddAsync(RoleCreateUpdateViewModel request)
    {
        if (request == null)
        {
            _logger.LogError("Role create view model is null.");
            return false;
        }

        // create a new role entity
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            IsActive = request.IsActive
        };

        // create the role in the database
        var result = await _roleManager.CreateAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Role created: {RoleName}", role.Name);
            return true;
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
        }

        return false;
    }

    /// <summary>
    /// Updates an existing role asynchronously.
    /// </summary>
    /// <param name="id">The ID of the role to update.</param>
    /// <param name="request">The view model containing role update data.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the role was updated successfully.</returns>
    public async Task<bool> UpdateAsync(Guid id, RoleCreateUpdateViewModel request)
    {
        if (request == null)
        {
            _logger.LogError("Role edit view model is null.");
            return false;
        }

        // get the role entity
        var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);

        if (role == null)
        {
            _logger.LogWarning("Role not found: {RoleId}", id);
            return false;
        }

        // update the role entity
        role.Name = request.Name;
        role.Description = request.Description;
        role.IsActive = request.IsActive;

        // update the role in the database
        var result = await _roleManager.UpdateAsync(role);

        if (result.Succeeded)
        {
            _logger.LogInformation("Role updated: {RoleName}", role.Name);
            return true;
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
        }

        return false;
    }
}
