using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Business.ViewModels.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController(IBookingService bookingService, IRoomService roomService, ILogger<BookingsController> logger) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IRoomService _roomService = roomService;
    private readonly ILogger<BookingsController> _logger = logger;

    [HttpGet("search-room")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchAvailableRoom(SearchingAvailableRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rooms = await _roomService.SearchAvailableRoomAsync(request);

        return Ok(rooms);
    }

    [HttpPost("booking-amenity")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> BookingAmenity(BookingAmenityRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Booking Amenity Request is invalid");
            return BadRequest(ModelState);
        }

        var result = await _bookingService.BookingAmenityAsync(request);

        _logger.LogInformation("Booking Amenity Request is successful");
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> BookingRoom(BookingRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Booking Room Request is invalid");
            return BadRequest(ModelState);
        }

        var result = await _bookingService.BookingRoomAsync(request);

        _logger.LogInformation("Booking Room Request is successful");
        return Ok(result);
    }

    [HttpPost("checkin")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> CheckIn(CheckInRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Check In is invalid");
            return BadRequest(ModelState);
        }

        var result = await _bookingService.CheckInAsync(request);

        _logger.LogInformation("Check In is successful");
        return Ok(result);
    }

    [HttpPost("checkout")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> CheckOut(CheckOutRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError("Check Out is invalid");
            return BadRequest(ModelState);
        }

        var result = await _bookingService.CheckOutAsync(request);

        _logger.LogInformation("Check Out is successful");
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(bookings);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (!result)
        {
            _logger.LogError("Delete Booking failed");
            return BadRequest();
        }

        _logger.LogInformation("Delete Booking successful");
        return Ok(result);
    }
}