using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Amenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AmenitiesController(IAmenityService amenityService) : ControllerBase
{
    private readonly IAmenityService _amenityService = amenityService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var amenities = await _amenityService.GetAllAsync();

        return Ok(amenities);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
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
    [Authorize(Roles = "Admin, Staff")]
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
    [Authorize(Roles = "Admin, Staff")]
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
    [Authorize(Roles = "Admin, Staff")]
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