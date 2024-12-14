using System.ComponentModel.DataAnnotations;

namespace HotelApp.Business.ViewModels.User;

public class UserEditViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters", MinimumLength = 1)]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [StringLength(50, ErrorMessage = "Last Name must be between {2} and {1} characters", MinimumLength = 1)]
    public required string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Phone Number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    [StringLength(12, ErrorMessage = "Phone Number must be between {2} and {1} characters", MinimumLength = 10)]
    public required string PhoneNumber { get; set; }

    public bool IsActive { get; set; }
}