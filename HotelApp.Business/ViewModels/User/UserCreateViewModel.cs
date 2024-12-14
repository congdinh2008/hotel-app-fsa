using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.User;

public class UserCreateViewModel
{
    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters", MinimumLength = 1)]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name must be between {2} and {1} characters", MinimumLength = 1)]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [StringLength(50, ErrorMessage = "Email must be between {2} and {1} characters", MinimumLength = 1)]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, ErrorMessage = "User Name must be between {2} and {1} characters", MinimumLength = 1)]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(20, ErrorMessage = "Password must be between {2} and {1} characters", MinimumLength = 8)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]
    [StringLength(20, ErrorMessage = "Confirm Password must be between {2} and {1} characters", MinimumLength = 8)]
    public required string ConfirmPassword { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    [StringLength(12, ErrorMessage = "Phone Number must be between {2} and {1} characters", MinimumLength = 10)]
    public required string PhoneNumber { get; set; }

    public bool IsActive { get; set; }
}
