using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.User;

public class UserUpdateRequest
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is not valid")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "First name must be between 1 and 50 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Last name must be between 1 and 50 characters")]
    public required string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public string? Avatar { get; set; }
}
