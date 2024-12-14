using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.User;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Business.Services;

public class UserService(IUnitOfWork unitOfWork, UserManager<User> userManager) : 
    BaseService<User>(unitOfWork), IUserService
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
}
