using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.User;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "User Id is required")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Current password is required")]
    public required string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New password is required")]
    [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
    public required string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm password is required")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public required string ConfirmPassword { get; set; }
}
