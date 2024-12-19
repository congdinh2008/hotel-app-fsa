using HotelApp.Business.Extensions;
using HotelApp.Business.Services.Base;
using HotelApp.Business.ViewModels;
using HotelApp.Business.ViewModels.Amenity;
using HotelApp.Data.Models;
using HotelApp.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HotelApp.Business.Services;

public class AmenityService(IUnitOfWork unitOfWork, ILogger<AmenityService> logger) :
    BaseService<Amenity>(unitOfWork, logger), IAmenityService
{

    public Task<int> AddAsync(AmenityCreateUpdateViewModel amenityCreateViewModel)
    {
        // Check if amenity is null
        if (amenityCreateViewModel == null)
        {
            _logger.LogError("Amenity is null");
            throw new ArgumentNullException(nameof(amenityCreateViewModel), "Amenity is null");
        }

        // Check if amenity number is already taken
        if (_unitOfWork.AmenityRepository.GetQuery().Any(r => r.Name == amenityCreateViewModel.Name))
        {
            _logger.LogError("Amenity with number {AmenityNumber} already exists", amenityCreateViewModel.Name);
            throw new ArgumentException($"Amenity with number {amenityCreateViewModel.Name} already exists");
        }

        // Create new amenity
        var amenity = new Amenity
        {
            Name = amenityCreateViewModel.Name,
            Description = amenityCreateViewModel.Description,
            Price = amenityCreateViewModel.Price,
            IsActive = amenityCreateViewModel.IsActive
        };

        // Add amenity to database
        _unitOfWork.AmenityRepository.Add(amenity);
        return _unitOfWork.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, AmenityCreateUpdateViewModel amenityUpdateViewModel)
    {
        // Check if amenity is null
        if (amenityUpdateViewModel == null)
        {
            _logger.LogError("Amenity is null");
            throw new ArgumentNullException(nameof(amenityUpdateViewModel), "Amenity is null");
        }

        // Get amenity by id
        var amenity = await _unitOfWork.AmenityRepository.GetByIdAsync(id);

        // Check if amenity exists
        if (amenity == null)
        {
            _logger.LogError("Amenity with id {AmenityId} not found", id);
            throw new ArgumentException($"Amenity with id {id} not found");
        }

        // Check if amenity number is already taken
        if (await _unitOfWork.AmenityRepository.GetQuery().AnyAsync(r => r.Name == amenityUpdateViewModel.Name && r.Id != id))
        {
            _logger.LogError("Amenity with number {AmenityNumber} already exists", amenityUpdateViewModel.Name);
            throw new ArgumentException($"Amenity with number {amenityUpdateViewModel.Name} already exists");
        }

        // Update amenity
        amenity.Name = amenityUpdateViewModel.Name;
        amenity.Description = amenityUpdateViewModel.Description;
        amenity.Price = amenityUpdateViewModel.Price;
        amenity.IsActive = amenityUpdateViewModel.IsActive;

        // Update amenity in database
        _unitOfWork.AmenityRepository.Update(amenity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }

    public async new Task<List<AmenityViewModel>> GetAllAsync()
    {
        var amenities = await base.GetAllAsync();

        // Map Amenity to AmenityViewModel
        var amenityViewModels = amenities.Select(amenity => new AmenityViewModel
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Description = amenity.Description,
            Price = amenity.Price,
            IsActive = amenity.IsActive
        }).ToList();

        return amenityViewModels;
    }

    public async new Task<AmenityViewModel> GetByIdAsync(Guid id)
    {
        var amenity = await base.GetByIdAsync(id);

        if (amenity == null)
        {
            _logger.LogError("Amenity with id {AmenityId} not found", id);
            throw new ArgumentException($"Amenity with id {id} not found");
        }

        // Map Amenity to AmenityViewModel
        var amenityViewModel = new AmenityViewModel
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Description = amenity.Description,
            Price = amenity.Price,
            IsActive = amenity.IsActive
        };

        return amenityViewModel;
    }

    public async Task<PaginatedResult<AmenityViewModel>> SearchAsync(SearchAmenityQuery request)
    {
        // Get query
        var query = _unitOfWork.AmenityRepository.GetQuery();

        // Filter by name
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(r => r.Name.Contains(request.Name));
        }

        // Filter by min price
        if (request.MinPrice.HasValue)
        {
            query = query.Where(r => r.Price >= request.MinPrice);
        }

        // Filter by max price
        if (request.MaxPrice.HasValue)
        {
            query = query.Where(r => r.Price <= request.MaxPrice);
        }

        // Order by
         query = !string.IsNullOrEmpty(request.OrderBy)
            ? query.OrderByExtensition(request.OrderBy, request.OrderDirection.ToString())
            : query.OrderBy(o => o.Name);

        // Get total count
        var totalCount = await query.CountAsync();

        // Paginate
        query = query.Skip((request.Page - 1) * request.Size).Take(request.Size);

        // Map Amenity to AmenityViewModel
        var result = await query.Select(amenity => new AmenityViewModel
        {
            Id = amenity.Id,
            Name = amenity.Name,
            Description = amenity.Description,
            Price = amenity.Price,
            IsActive = amenity.IsActive
        }).ToListAsync();

        return new PaginatedResult<AmenityViewModel>(result, totalCount, request.Page, request.Size);
    }
}