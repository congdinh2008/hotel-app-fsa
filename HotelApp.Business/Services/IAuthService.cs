using HotelApp.Business.ViewModels.Auth;

namespace HotelApp.Business.Services;

public interface IAuthService
{
    Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel);

    Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel);
}
