using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginViewModel);

        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAsync(registerViewModel);

        if (result == null)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}