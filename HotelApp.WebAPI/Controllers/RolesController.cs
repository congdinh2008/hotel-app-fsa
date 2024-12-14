using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Role;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
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