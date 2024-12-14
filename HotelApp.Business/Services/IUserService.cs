using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels.User;
using HotelApp.Data.Models;

namespace HotelApp.Business.Services;

public interface IUserService : IBaseService<User>
{
    Task<bool> UpdatePasswordAsync(Guid userId, string oldPassword, string newPassword);

    Task<bool> UpdateProfileAsync(Guid userId, UserUpdateRequest request);

    Task<bool> CreateUserAsync(UserCreateRequest request, string password);

    Task<bool> CreateUserAsync(UserCreateViewModel request);

    Task<bool> RemoveRoleAsync(Guid userId, string role);

    Task<bool> AddToRoleAsync(Guid userId, string role);

    Task<bool> SetPasswordAsync(Guid userId, string password);

    Task<UserViewModel?> GetUserByEmailAsync(string email);

    Task<bool> ChangePasswordAsync(ChangePasswordViewModel request);

    Task<bool> UpdateUserAsync(Guid id, UserEditViewModel request);
}
