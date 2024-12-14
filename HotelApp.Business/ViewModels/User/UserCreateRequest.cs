using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.User;

public class UserCreateRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    public required string ConfirmPassword { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Avatar { get; set; }
}
