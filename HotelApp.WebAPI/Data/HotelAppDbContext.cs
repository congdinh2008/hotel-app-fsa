using HotelApp.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.WebAPI.Data;

public class HotelAppDbContext(DbContextOptions<HotelAppDbContext> options) : DbContext(options)
{
    public DbSet<Room> Rooms { get; set; } = default!;
}
