using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Booking;
using HotelApp.Business.ViewModels.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller for managing bookings.
/// </summary>
/// <param name="bookingService">Booking service.</param>
/// <param name="roomService">Room service.</param>
/// <param name="logger">Logger.</param>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Tags("Bookings")]
public class BookingsController(IBookingService bookingService, IRoomService roomService, ILogger<BookingsController> logger) : ControllerBase
{
    private readonly IBookingService _bookingService = bookingService;
    private readonly IRoomService _roomService = roomService;
    private readonly ILogger<BookingsController> _logger = logger;

    /// <summary>
    /// Retrieves all available rooms.
    /// </summary>
    /// <param name="request">The request to search for available rooms between check-in and check-out dates.</param>
    /// <returns>A list of available rooms.</returns>
    [HttpGet("search-room")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<RoomViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAvailableRoom(SearchingAvailableRoomRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var rooms = await _roomService.SearchAvailableRoomAsync(request);

        return Ok(rooms);
    }

    /// <summary>
    /// Books an amenity.
    /// </summary>
    /// <param name="request">The booking amenity request.</param>
    /// <returns>The booking amenity view model.</returns>
    [HttpPost("booking-amenity")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(BookingAmenityViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Books a room.
    /// </summary>
    /// <param name="request">The request to book a room based on the check-in and check-out dates.</param>
    /// <returns>The booking room view model.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Checks in a booking.
    /// </summary>
    /// <param name="request">The check-in request.</param>
    /// <returns>The check-in result.</returns>
    [HttpPost("checkin")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Checks out a booking.
    /// </summary>
    /// <param name="request">The check-out request.</param>
    /// <returns>The check-out result.</returns>
    [HttpPost("checkout")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(CheckOutViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    /// <summary>
    /// Retrieves all bookings.
    /// </summary>
    /// <returns>A list of bookings.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(List<BookingViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var bookings = await _bookingService.GetAllAsync();

        return Ok(bookings);
    }

    /// <summary>
    /// Retrieves a booking by its ID.
    /// </summary>
    /// <param name="id">The ID of the booking.</param>
    /// <returns>The requested booking.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(BookingViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
        {
            return NotFound();
        }

        return Ok(booking);
    }

    /// <summary>
    /// Deletes a booking.
    /// </summary>
    /// <param name="id">The ID of the booking.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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