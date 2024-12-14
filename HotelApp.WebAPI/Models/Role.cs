using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HotelApp.WebAPI.Models;

[Table("Roles")]
public class Role: IdentityRole<Guid>
{
    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
