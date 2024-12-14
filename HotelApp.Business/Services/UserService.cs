using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.User;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class UserService(IUnitOfWork unitOfWork, ILogger<UserService> logger, UserManager<User> userManager) :
    BaseService<User>(unitOfWork, logger), IUserService
{
    private readonly UserManager<User> _userManager = userManager;

    public new async Task<UserViewModel?> GetByIdAsync(Guid id)
    {
        var user = await base.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return new UserViewModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DisplayName = user.DisplayName,
            DateOfBirth = user.DateOfBirth,
            Avatar = user.Avatar,
            IsActive = user.IsActive
        };
    }

    // Hide the base method AddAsync
    public new async Task<bool> AddAsync(User entity)
    {
        var result = await _userManager.CreateAsync(entity);

        return result.Succeeded;
    }

    // Hide the base method UpdateAsync
    public new async Task<bool> UpdateAsync(User entity)
    {
        var result = await _userManager.UpdateAsync(entity);

        return result.Succeeded;
    }

    // Hide the base method DeleteAsync
    public new async Task<bool> DeleteAsync(User entity)
    {
        var result = await _userManager.DeleteAsync(entity);

        return result.Succeeded;
    }

    // Hide the base method GetAllAsync
    public new async Task<IEnumerable<UserViewModel>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        return users.Select(user => new UserViewModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DisplayName = user.DisplayName,
            DateOfBirth = user.DateOfBirth,
            Avatar = user.Avatar,
            IsActive = user.IsActive
        });
    }

    public async Task<bool> AddToRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        var result = await _userManager.AddToRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> CreateUserAsync(UserCreateRequest request, string password)
    {
        var user = new User
        {
            Email = request.Email,
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Avatar = request.Avatar,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded;
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="request">The view model containing user creation data.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the user was created successfully.</returns>
    public async Task<bool> CreateUserAsync(UserCreateViewModel request)
    {
        // create a new user entity
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            IsActive = request.IsActive
        };

        // create the user in the database
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("User created: {UserName}", user.UserName);
            return true;
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
        }

        return false;
    }

    public async Task<UserViewModel?> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        return new UserViewModel
        {
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DisplayName = user.DisplayName,
            DateOfBirth = user.DateOfBirth,
            Avatar = user.Avatar,
            IsActive = user.IsActive
        };
    }

    public async Task<bool> RemoveRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        var result = await _userManager.RemoveFromRoleAsync(user, role);

        return result.Succeeded;
    }

    public async Task<bool> SetPasswordAsync(Guid userId, string password)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, token, password);

        return result.Succeeded;
    }

    public async Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

        return result.Succeeded;
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UserUpdateRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return false;
        }

        user.Email = request.Email;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.DateOfBirth = request.DateOfBirth;
        user.Avatar = request.Avatar;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel request)
    {
        // Check if the view model is null
        if (request == null)
        {
            _logger.LogError("Change password view model is null.");
            return false;
        }

        // Get the user entity
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
        {
            _logger.LogWarning("User not found: {UserName}", request.UserName);
            return false;
        }

        if (request.NewPassword != request.ConfirmPassword)
        {
            _logger.LogWarning("New password and confirm password do not match.");
            return false;
        }

        // Change the user's password
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
        {
            _logger.LogInformation("Password changed for user: {UserName}", user.UserName);
            return true;
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
        }

        return false;
    }



    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="request">The view model containing user update data.</param>
    /// <returns>A task representing the asynchronous operation. The task result indicates whether the user was updated successfully.</returns>
    public async Task<bool> UpdateUserAsync(Guid id, UserEditViewModel request)
    {
        // get the user entity
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", id);
            return false;
        }

        // update the user entity
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumber = request.PhoneNumber;
        user.DateOfBirth = request.DateOfBirth;
        user.IsActive = request.IsActive;

        // update the user in the database
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User updated: {UserName}", user.UserName);
            return true;
        }

        foreach (var error in result.Errors)
        {
            _logger.LogError(error.Description);
        }

        return false;
    }
}
