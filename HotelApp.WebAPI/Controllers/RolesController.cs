using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
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

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
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

    [HttpPost]
    [Authorize(Roles = "Admin")]
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

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
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