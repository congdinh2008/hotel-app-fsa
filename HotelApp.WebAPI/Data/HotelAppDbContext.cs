using HotelApp.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.WebAPI.Data;

public class HotelAppDbContext(DbContextOptions<HotelAppDbContext> options) : IdentityDbContext<User, Role, Guid>(options)
{
    public DbSet<Room> Rooms { get; set; } = default!;

    public DbSet<Amenity> Amenities { get; set; } = default!;

    public DbSet<Booking> Bookings { get; set; } = default!;

    public DbSet<BookingDetail> BookingDetails { get; set; } = default!;

    public DbSet<BookingAmenity> BookingAmenities { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<Role>().ToTable("Roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
    }
}
