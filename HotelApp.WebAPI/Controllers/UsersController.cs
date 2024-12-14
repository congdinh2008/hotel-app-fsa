using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();

        var userViewModels = users.Select(user => new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            IsActive = user.IsActive,
        }).ToList();

        return Ok(userViewModels);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            DisplayName = user.DisplayName ?? string.Empty,
            IsActive = user.IsActive
        };
        return Ok(userViewModel);
    }

    [HttpPost("/changePassword")]
    [Authorize(Roles = "Admin, Staff, Customer")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool result = await _userService.ChangePasswordAsync(changePasswordViewModel);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser(UserCreateViewModel userCreateViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool result = await _userService.CreateUserAsync(userCreateViewModel);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> UpdateUser(Guid id, UserEditViewModel userEditViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        bool result = await _userService.UpdateUserAsync(id, userEditViewModel);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        bool result = await _userService.DeleteAsync(id);

        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}