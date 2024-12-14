using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller responsible for handling user-related actions.
/// </summary>
/// <param name="userService">User service.</param>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Tags("Users")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(IEnumerable<UserViewModel>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Retrieves a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>The requested user.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

    /// <summary>
    /// Changes the password of the user.
    /// </summary>
    /// <param name="changePasswordViewModel">The change password view model containing the user ID and new password.</param>
    /// <returns>A boolean indicating whether the password change was successful.</returns>
    [HttpPost("/changePassword")]
    [Authorize(Roles = "Admin, Staff, Customer")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userCreateViewModel">The user details.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="userEditViewModel">The updated user details.</param>
    /// <returns>True if the user was updated successfully; otherwise, false.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Deletes a user by its ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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