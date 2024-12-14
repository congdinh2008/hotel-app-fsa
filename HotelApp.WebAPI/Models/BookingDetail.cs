using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.WebAPI.Models;

[Table("BookingDetails")]
[PrimaryKey(nameof(BookingId), nameof(RoomId))]
public class BookingDetail
{
    [Required]
    [ForeignKey(nameof(Booking))]
    public Guid BookingId { get; set; }

    public Booking? Booking { get; set; }

    [Required]
    [ForeignKey(nameof(Room))]
    public Guid RoomId { get; set; }

    public Room? Room { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
}