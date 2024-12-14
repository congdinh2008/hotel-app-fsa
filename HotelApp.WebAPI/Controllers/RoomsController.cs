using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller for managing rooms.
/// </summary>
[Authorize]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Tags("Rooms")]
public class RoomsController(IRoomService roomService) : ControllerBase
{
    private readonly IRoomService _roomService = roomService;

    /// <summary>
    /// Retrieves all rooms.
    /// </summary>
    /// <returns>A list of rooms.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<RoomViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();

        return Ok(rooms);
    }

    /// <summary>
    /// Retrieves a room by its ID.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <returns>The room with the specified ID.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RoomViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomService.GetByIdAsync(id);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    /// <summary>
    /// Creates a new room.
    /// </summary>
    /// <param name="request">The room details.</param>
    /// <returns>A boolean indicating whether the creation was successful.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(RoomCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.AddAsync(request);

        return Ok(result > 0);
    }

    /// <summary>
    /// Updates an existing room.
    /// </summary>
    /// <param name="id">The ID of the room to update.</param>
    /// <param name="request">The updated room details.</param>
    /// <returns>A boolean indicating whether the update was successful.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, RoomCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.UpdateAsync(id, request);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a room by its ID.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var result = await _roomService.DeleteAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}