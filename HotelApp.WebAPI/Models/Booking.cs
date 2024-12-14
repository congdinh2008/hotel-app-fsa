using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelApp.WebAPI.Enums;

namespace HotelApp.WebAPI.Models;

[Table("Bookings")]
public class Booking
{
    public Guid Id { get; set; }

    [Required]
    public required DateTimeOffset BookingDate { get; set; }

    [Required]
    public required DateTimeOffset CheckInDate { get; set; }

    [Required]
    public required DateTimeOffset CheckOutDate { get; set; }

    [Required]
    public required BookingStatus Status { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(Customer))]
    public Guid CustomerId { get; set; }

    public required User Customer { get; set; }

    public ICollection<BookingDetail> BookingDetails { get; set; } = [];
}
