using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller responsible for handling role-related actions.
/// </summary>
/// <param name="roleService"></param>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Roles")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    /// <summary>
    /// Retrieves all roles.
    /// </summary>
    /// <returns>A list of roles.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(IEnumerable<RoleViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _roleService.GetAllAsync();

        var roleViewModels = roles.Select(question => new RoleViewModel
        {
            Id = question.Id,
            Name = question.Name ?? string.Empty,
            Description = question.Description ?? string.Empty,
            IsActive = question.IsActive,
        }).ToList();

        return Ok(roleViewModels);
    }

    /// <summary>
    /// Retrieves a role by its ID.
    /// </summary>
    /// <param name="id">The ID of the role.</param>
    /// <returns>The requested role.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(RoleViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRole(Guid id)
    {
        var role = await _roleService.GetByIdAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        var roleViewModel = new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description ?? string.Empty,
            IsActive = role.IsActive
        };
        return Ok(roleViewModel);
    }

    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="request">The role details.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole(RoleCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool result = await _roleService.AddAsync(request);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    /// <summary>
    /// Updates an existing role.
    /// </summary>
    /// <param name="id">The ID of the role to update.</param>
    /// <param name="request">The updated role details.</param>
    /// <returns>True if the role was updated successfully; otherwise, false.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRole(Guid id, RoleCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool result = await _roleService.UpdateAsync(id, request);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    /// <summary>
    /// Deletes a role by its ID.
    /// </summary>
    /// <param name="id">The ID of the role to delete.</param>
    /// <returns>True if the role was deleted successfully; otherwise, false.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        bool result = await _roleService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}