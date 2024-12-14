using System;
using HotelApp.WebAPI.Enums;
using HotelApp.WebAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelApp.WebAPI.Data;

public static class DbInitializer
{
    public static void SeedData(HotelAppDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        // Seed Users and Roles - Optional
        var admin = new User { Id = Guid.NewGuid(), UserName = "admin", Email = "admin@domain.com", FirstName = "Admin", LastName = "User", PhoneNumber = "1234567890", DateOfBirth = DateTime.Now.AddDays(-3650), Avatar = "avatar-1.png", IsActive = true };
        var staff = new User { Id = Guid.NewGuid(), UserName = "editor", Email = "editor@domain.com", FirstName = "Editor", LastName = "User", PhoneNumber = "1234567890", DateOfBirth = DateTime.Now.AddDays(-4650), Avatar = "avatar-2.png", IsActive = true };
        var cong = new User { Id = Guid.NewGuid(), UserName = "congdinh", Email = "cong@domain.com", FirstName = "Cong", LastName = "Dinh", PhoneNumber = "1234567890", DateOfBirth = DateTime.Now.AddDays(-5650), Avatar = "avatar-3.png", IsActive = true };
        var van = new User { Id = Guid.NewGuid(), UserName = "vannguyen", Email = "van@domain.com", FirstName = "Van", LastName = "Nguyen", PhoneNumber = "1234567890", DateOfBirth = DateTime.Now.AddDays(-6650), Avatar = "avatar-4.png", IsActive = true };
        var quynh = new User { Id = Guid.NewGuid(), UserName = "quynhdinh", Email = "quynh@domain.com", FirstName = "Quynh", LastName = "Dinh", PhoneNumber = "1234567890", DateOfBirth = DateTime.Now.AddDays(-7650), Avatar = "avatar-5.png", IsActive = true };

        if (!context.Users.Any())
        {
            var roles = new List<Role>
            {
                new Role { Id= Guid.NewGuid(), Name = "Admin", Description = "Administrator", IsActive = true },
                new Role { Id= Guid.NewGuid(), Name = "Staff", Description = "Staff", IsActive = true },
                new Role { Id= Guid.NewGuid(), Name = "Customer", Description = "Customer", IsActive = true }
            };

            foreach (var role in roles)
            {
                roleManager.CreateAsync(role).Wait();
            }

            userManager.CreateAsync(admin, "Admin@1234").Wait();
            userManager.AddToRoleAsync(admin, "Admin").Wait();

            userManager.CreateAsync(staff, "Staff@1234").Wait();
            userManager.AddToRoleAsync(staff, "Staff").Wait();

            userManager.CreateAsync(cong, "Customer@1234").Wait();
            userManager.AddToRoleAsync(cong, "Customer").Wait();

            userManager.CreateAsync(van, "Customer@1234").Wait();
            userManager.AddToRoleAsync(van, "Customer").Wait();

            userManager.CreateAsync(quynh, "Customer@1234").Wait();
            userManager.AddToRoleAsync(quynh, "Customer").Wait();
        }

        // Seed Common Entities
        List<Room> rooms = [
           new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "101",
                    Type = RoomType.Standard,
                    Capacity = 2,
                    IsActive = true,
                    PricePerNight = 100,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "102",
                    Type = RoomType.Standard,
                    Capacity = 3,
                    IsActive = true,
                    PricePerNight = 120,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "103",
                    Type = RoomType.Standard,
                    Capacity = 4,
                    IsActive = true,
                    PricePerNight = 150,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "104",
                    Type = RoomType.Superior,
                    Capacity = 3,
                    IsActive = true,
                    PricePerNight = 150,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "105",
                    Type = RoomType.Superior,
                    Capacity = 4,
                    IsActive = true,
                    PricePerNight = 200,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "106",
                    Type = RoomType.Deluxe,
                    Capacity = 4,
                    IsActive = true,
                    PricePerNight = 220,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "107",
                    Type = RoomType.Deluxe,
                    Capacity = 5,
                    IsActive = true,
                    PricePerNight = 250,
                    Status = RoomStatus.Available
                },
            new Room
                {
                    Id = Guid.NewGuid(),
                    Number = "108",
                    Type = RoomType.Suite,
                    Capacity = 5,
                    IsActive = true,
                    PricePerNight = 250,
                    Status = RoomStatus.Available
                },
        ];

        if (!context.Rooms.Any())
        {
            context.Rooms.AddRange(rooms);
            context.SaveChanges();
        }

        // Seed Room Services
        List<Amenity> roomServices = [
            new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = "Breakfast",
                    Description = "Breakfast in the room",
                    Price = 10,
                    IsActive = true
                },
            new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = "Laundry",
                    Description = "Laundry service",
                    Price = 20,
                    IsActive = true
                },
            new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = "Airport Transfer",
                    Description = "Airport transfer service",
                    Price = 30,
                    IsActive = true
                },
            new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = "Car Rental",
                    Description = "Car rental service",
                    Price = 40,
                    IsActive = true
                },
            new Amenity
                {
                    Id = Guid.NewGuid(),
                    Name = "Spa",
                    Description = "Spa service",
                    Price = 50,
                    IsActive = true
                },
        ];

        if (!context.Amenities.Any())
        {
            context.Amenities.AddRange(roomServices);
            context.SaveChanges();
        }

        // Seed Bookings
        List<Booking> bookings = [
            new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingDate = DateTime.Now,
                    CheckInDate = DateTime.Now.AddDays(1),
                    CheckOutDate = DateTime.Now.AddDays(3),
                    Status = BookingStatus.Confirmed,
                    IsActive = true,
                    CustomerId = cong.Id,
                    Customer = cong,
                    BookingDetails = [
                        new BookingDetail
                        {
                            RoomId = rooms[0].Id,
                            Price = 100
                        },
                        new BookingDetail
                        {
                            RoomId = rooms[1].Id,
                            Price = 120
                        }
                    ]
                },
            new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingDate = DateTime.Now,
                    CheckInDate = DateTime.Now.AddDays(2),
                    CheckOutDate = DateTime.Now.AddDays(4),
                    Status = BookingStatus.Pending,
                    IsActive = true,
                    CustomerId = van.Id,
                    Customer = van,
                    BookingDetails = [
                        new BookingDetail
                        {
                            RoomId = rooms[2].Id,
                            Price = 150
                        },
                        new BookingDetail
                        {
                            RoomId = rooms[3].Id,
                            Price = 200
                        }
                    ]
                },
            new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingDate = DateTime.Now,
                    CheckInDate = DateTime.Now.AddDays(3),
                    CheckOutDate = DateTime.Now.AddDays(5),
                    Status = BookingStatus.Cancelled,
                    IsActive = true,
                    CustomerId = quynh.Id,
                    Customer = quynh,
                    BookingDetails = [
                        new BookingDetail
                        {
                            RoomId = rooms[4].Id,
                            Price = 250
                        },
                        new BookingDetail
                        {
                            RoomId = rooms[5].Id,
                            Price = 220
                        }
                    ]
                },
        ];

        if (!context.Bookings.Any())
        {
            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }

        // Seed Booking Services
        List<BookingAmenity> bookingServices = [
            new BookingAmenity
                {
                    BookingId = bookings[0].Id,
                    RoomId = rooms[0].Id,
                    AmenityId = roomServices[0].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 2,
                    Price = 20
                },
            new BookingAmenity
                {
                    BookingId = bookings[0].Id,
                    RoomId = rooms[1].Id,
                    AmenityId = roomServices[1].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 1,
                    Price = 20
                },
            new BookingAmenity
                {
                    BookingId = bookings[1].Id,
                    RoomId = rooms[2].Id,
                    AmenityId = roomServices[2].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 1,
                    Price = 30
                },
            new BookingAmenity
                {
                    BookingId = bookings[1].Id,
                    RoomId = rooms[3].Id,
                    AmenityId = roomServices[3].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 2,
                    Price = 80
                },
            new BookingAmenity
                {
                    BookingId = bookings[2].Id,
                    RoomId = rooms[4].Id,
                    AmenityId = roomServices[4].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 1,
                    Price = 50
                },
            new BookingAmenity
                {
                    BookingId = bookings[2].Id,
                    RoomId = rooms[5].Id,
                    AmenityId = roomServices[0].Id,
                    BookingDate = DateTime.Now,
                    Quantity = 2,
                    Price = 40
                }
        ];

        if (!context.BookingAmenities.Any())
        {
            context.BookingAmenities.AddRange(bookingServices);
            context.SaveChanges();
        }
    }
}

