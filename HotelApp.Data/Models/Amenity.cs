using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp.Data.Models;

[Table("Amenities")]
public class Amenity
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Required]
    public bool IsActive { get; set; }
}
