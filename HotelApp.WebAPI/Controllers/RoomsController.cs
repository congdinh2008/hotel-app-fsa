using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Room;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;

    public RoomsController(IRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rooms = await _roomService.GetAllAsync();

        return Ok(rooms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var room = await _roomService.GetByIdAsync(id);

        if (room == null)
        {
            return NotFound();
        }

        return Ok(room);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoomCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.AddAsync(request);

        return Ok(result > 0);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RoomCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _roomService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
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