using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Amenity;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly IAmenityService _amenityService;

    public AmenitiesController(IAmenityService amenityService)
    {
        _amenityService = amenityService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var amenities = await _amenityService.GetAllAsync();

        return Ok(amenities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var amenity = await _amenityService.GetByIdAsync(id);

        if (amenity == null)
        {
            return NotFound();
        }

        return Ok(amenity);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AmenityCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _amenityService.AddAsync(request);

        return Ok(result > 0);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, AmenityCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _amenityService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        var result = await _amenityService.DeleteAsync(id);
        if (!result)
        {
            return BadRequest();
        }

        return Ok(result);
    }
}