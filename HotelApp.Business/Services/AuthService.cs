using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using HotelApp.Business.ViewModels.Auth;
using HotelApp.Business.ViewModels.User;
using HotelApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelApp.Business.Services;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
   
    private readonly SignInManager<User> _signInManager = signInManager;
   
    private readonly IConfiguration _configuration = configuration;

    public async Task<LoginResponseViewModel> LoginAsync(LoginViewModel loginViewModel)
    {
        var existingUser = await _userManager.FindByNameAsync(loginViewModel.UserName) ??
            throw new ArgumentException("The user does not exist.");

        var result = await _signInManager.CheckPasswordSignInAsync(existingUser, loginViewModel.Password, false);

        if (!result.Succeeded)
        {
            throw new ArgumentException("The password is incorrect.");
        }

        var userViewModel = new UserViewModel
        {
            Id = existingUser.Id,
            FirstName = existingUser.FirstName,
            LastName = existingUser.LastName,
            DisplayName = existingUser.DisplayName ?? string.Empty,
            Email = existingUser.Email ?? string.Empty,
            UserName = existingUser.UserName ?? string.Empty,
            PhoneNumber = existingUser.PhoneNumber ?? string.Empty,
            IsActive = existingUser.IsActive
        };

        var userJson = GetSerializeObject(userViewModel);

        var roles = await _userManager.GetRolesAsync(existingUser);
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, existingUser.Id.ToString()),
            new(ClaimTypes.Name, existingUser.UserName ?? string.Empty),
            new(ClaimTypes.Email, existingUser.Email ?? string.Empty),
            new(ClaimTypes.GivenName, existingUser.FirstName),
            new(ClaimTypes.Surname, existingUser.LastName),
            new(ClaimTypes.Role, string.Join(",", roles))
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? "congdinh2021@gmail.com"));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            audience: _configuration["Jwt:ValidAudience"],
            claims: claims,
            expires: _configuration["Jwt.ExpirationInMinutes"] == null ? DateTime.Now.AddDays(1) : DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt.ExpirationInMinutes"])),
            signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
        );

        return new LoginResponseViewModel
        {
            UserInformation = userJson,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expires = token.ValidTo
        };
    }

    private string GetSerializeObject(object value)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        return value != null ? JsonSerializer.Serialize(value, serializeOptions) : string.Empty;
    }

    public async Task<LoginResponseViewModel> RegisterAsync(RegisterViewModel registerViewModel)
    {
        var existingUser = await _userManager.FindByNameAsync(registerViewModel.UserName);

        if (existingUser != null)
        {
            throw new ArgumentException("The user already exists.");
        }

        var user = new User
        {
            FirstName = registerViewModel.FirstName,
            LastName = registerViewModel.LastName,
            Email = registerViewModel.Email,
            UserName = registerViewModel.UserName,
            PhoneNumber = registerViewModel.PhoneNumber,
            DateOfBirth = registerViewModel.DateOfBirth,
            IsActive = registerViewModel.IsActive
        };

        var result = await _userManager.CreateAsync(user, registerViewModel.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error => error.Description);
            throw new ArgumentException($"The user could not be created. Errors: {string.Join(", ", errors)}");
        }

        return await LoginAsync(new LoginViewModel
        {
            UserName = registerViewModel.UserName,
            Password = registerViewModel.Password
        });
    }
}
