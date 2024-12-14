using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HotelApp.Data.Models;

[Table("Users")]
public class User: IdentityUser<Guid>
{
    [Required]
    [MaxLength(50)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public required string LastName { get; set; }

    [NotMapped]
    public string DisplayName => $"{FirstName} {LastName}";

    public DateTime DateOfBirth { get; set; }

    [MaxLength(1000)]
    public string? Avatar { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
