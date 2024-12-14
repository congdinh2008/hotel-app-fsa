using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller responsible for handling authentication-related actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Authentication")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Handles user login.
    /// </summary>
    /// <param name="loginViewModel">The login view model containing user credentials.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the login operation.</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    /// <summary>
    /// Handles user registration.
    /// </summary>
    /// <param name="registerViewModel">The register view model containing user registration details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the registration operation.</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(LoginResponseViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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