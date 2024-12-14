using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApp.WebAPI.Enums;

namespace HotelApp.WebAPI.Models;

[Table("Rooms")]
public class Room
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(10)]
    public required string Number { get; set; }

    [Required]
    public RoomType Type { get; set; }

    public int Capacity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal PricePerNight { get; set; }

    [Required]
    public RoomStatus Status { get; set; }
}
