using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.WebAPI.Models;

[Table("BookingAmenities")]
[PrimaryKey(nameof(BookingId), nameof(AmenityId), nameof(RoomId), nameof(BookingDate))]
public class BookingAmenity
{
    [Required]
    [ForeignKey(nameof(Booking))]
    public Guid BookingId { get; set; }

    public Booking? Booking { get; set; }

    [Required]
    [ForeignKey(nameof(Amenity))]
    public Guid AmenityId { get; set; }

    public Amenity? Amenity { get; set; }

    [Required]
    [ForeignKey(nameof(Room))]
    public Guid RoomId { get; set; }

    public Room? Room { get; set; }

    [Required]
    public required DateTimeOffset BookingDate { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
}