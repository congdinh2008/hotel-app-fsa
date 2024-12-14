using HotelApp.Business.Services;
using HotelApp.Business.ViewModels.Amenity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.WebAPI.Controllers;

/// <summary>
/// Controller for managing amenities.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Amenities")]
public class AmenitiesController(IAmenityService amenityService) : ControllerBase
{
    private readonly IAmenityService _amenityService = amenityService;

    /// <summary>
    /// Retrieves all amenities.
    /// </summary>
    /// <returns>A list of amenities.</returns>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<AmenityViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        var amenities = await _amenityService.GetAllAsync();

        return Ok(amenities);
    }

    /// <summary>
    /// Retrieves an amenity by its ID.
    /// </summary>
    /// <param name="id">The ID of the amenity.</param>
    /// <returns>The requested amenity.</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AmenityViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var amenity = await _amenityService.GetByIdAsync(id);

        if (amenity == null)
        {
            return NotFound();
        }

        return Ok(amenity);
    }

    /// <summary>
    /// Creates a new amenity.
    /// </summary>
    /// <param name="request">The amenity details.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    [HttpPost]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(AmenityCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _amenityService.AddAsync(request);

        return Ok(result > 0);
    }

    /// <summary>
    /// Updates an existing amenity.
    /// </summary>
    /// <param name="id">The ID of the amenity to update.</param>
    /// <param name="request">The updated amenity details.</param>
    /// <returns>The updated amenity.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, AmenityCreateUpdateViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _amenityService.UpdateAsync(id, request);

        return Ok(result);
    }

    /// <summary>
    /// Deletes an amenity by its ID.
    /// </summary>
    /// <param name="id">The ID of the amenity to delete.</param>
    /// <returns>A boolean indicating success or failure.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin, Staff")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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