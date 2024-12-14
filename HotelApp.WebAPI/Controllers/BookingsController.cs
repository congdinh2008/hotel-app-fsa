using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Business.ViewModels.Room;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IRoomService _roomService;

    public BookingsController(IBookingService bookingService, IRoomService roomService)
    {
        _bookingService = bookingService;
        _roomService = roomService;
    }

    [HttpGet("search-room")]
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
    public async Task<IActionResult> BookingAmenity(BookingAmenityRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.BookingAmenityAsync(request);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> BookingRoom(BookingRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.BookingRoomAsync(request);

        return Ok(result);
    }

    [HttpPost("checkin")]
    public async Task<IActionResult> CheckIn(CheckInRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.CheckInAsync(request);

        return Ok(result);
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckOut(CheckOutRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _bookingService.CheckOutAsync(request);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(bookings);
    }

    [HttpGet("{id}")]
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
    public async Task<IActionResult> DeleteBooking(Guid id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}