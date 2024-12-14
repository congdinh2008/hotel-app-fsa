using Moq;
using Microsoft.Extensions.Logging;
using HotelApp.Business.Services;
using HotelApp.Data.UnitOfWork;
using HotelApp.Data.Models;
using HotelApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using HotelApp.Data.Contexts;

namespace HotelApp.UnitTesting.Services;

[TestFixture]
public class AmenityServiceTest
{
    private Mock<HotelAppDbContext> _mockDbContext = null!;
    private Mock<DbSet<Amenity>> _mockDbSet = null!;
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<IGenericRepository<Amenity>> _mockRepository = null!;
    private Mock<ILogger<AmenityService>> _mockLogger = null!;
    private AmenityService _amenityService = null!;

    private List<Amenity> amenities = null!;


    [SetUp]
    public void Setup()
    {
        amenities = new List<Amenity> {
            new Amenity { Id = Guid.NewGuid(), Name = "WiFi" },
            new Amenity { Id = Guid.NewGuid(), Name = "Parking" }
        };

        _mockDbSet = new Mock<DbSet<Amenity>>();
        var amenityQueryable = amenities.AsQueryable();
        _mockDbSet.As<IQueryable<Amenity>>().Setup(m => m.Provider).Returns(amenityQueryable.Provider);
        _mockDbSet.As<IQueryable<Amenity>>().Setup(m => m.Expression).Returns(amenityQueryable.Expression);
        _mockDbSet.As<IQueryable<Amenity>>().Setup(m => m.ElementType).Returns(amenityQueryable.ElementType);
        _mockDbSet.As<IQueryable<Amenity>>().Setup(m => m.GetEnumerator()).Returns(amenityQueryable.GetEnumerator());
        _mockDbSet.As<IEnumerable<Amenity>>().Setup(m => m.GetEnumerator()).Returns(amenityQueryable.GetEnumerator());

        _mockDbContext = new Mock<HotelAppDbContext>();
        _mockDbContext.Setup(c => c.Set<Amenity>()).Returns(_mockDbSet.Object);

        _mockRepository = new Mock<IGenericRepository<Amenity>>();
        _mockRepository.Setup(r => r.GetAll()).Returns(amenityQueryable.AsEnumerable());
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(amenityQueryable.AsEnumerable());
        _mockRepository.Setup(r => r.GetQuery()).Returns(amenityQueryable);
        _mockRepository.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Guid id) => amenities.FirstOrDefault(a => a.Id == id));
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid id) => amenities.FirstOrDefault(a => a.Id == id));
        _mockRepository.Setup(r => r.Add(It.IsAny<Amenity>())).Callback((Amenity amenity) => amenities.Add(amenity));
        _mockRepository.Setup(r => r.Update(It.IsAny<Amenity>())).Callback((Amenity amenity) =>
        {
            var existingAmenity = amenities.FirstOrDefault(a => a.Id == amenity.Id);
            if (existingAmenity != null)
            {
                existingAmenity.Name = amenity.Name;
            }
        });
        _mockRepository.Setup(r => r.Delete(It.IsAny<Amenity>())).Callback((Amenity amenity) =>
        {
            var existingAmenity = amenities.FirstOrDefault(a => a.Id == amenity.Id);
            if (existingAmenity != null)
            {
                amenities.Remove(existingAmenity);
            }
        });

        _mockLogger = new Mock<ILogger<AmenityService>>();

        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(uow => uow.GenericRepository<Amenity>()).Returns(_mockRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _amenityService = new AmenityService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    [Test]
    public async Task GetAllAmenities_ReturnsAllAmenities()
    {
        var result = await _amenityService.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetAmenityById_ReturnsAmenity()
    {
        var amenity = amenities.AsEnumerable().First();
        var result = await _amenityService.GetByIdAsync(amenity.Id);

        Assert.That(result!.Id, Is.EqualTo(amenity.Id));
        Assert.That(result.Name, Is.EqualTo(amenity.Name));
    }

    [Test]
    public async Task AddAmenity_ReturnsAmenity()
    {
        var amenity = new Amenity { Id = Guid.NewGuid(), Name = "Pool" };
        await _amenityService.AddAsync(amenity);

        var result = await _amenityService.GetByIdAsync(amenity.Id);

        Assert.That(result!.Id, Is.EqualTo(amenity.Id));
        Assert.That(result.Name, Is.EqualTo(amenity.Name));
    }

    [Test]
    public void AddAmenityWithNull_ThrowsArgumentNullException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _amenityService.AddAsync(null!));

        _mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((object v, Type _) => v.ToString() == "Amenity is null"),
            null,
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
        ));
    }

    [Test]
    public async Task UpdateAmenity_ReturnsTrue()
    {
        var amenity = amenities[0];
        amenity.Name = "Gym";
        var result = await _amenityService.UpdateAsync(amenity);

        Assert.That(result, Is.True);
    }

    [Test]
    public void UpdateAmenityWithNull_ThrowsArgumentNullException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _amenityService.UpdateAsync(null!));

        _mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((object v, Type _) => v.ToString() == "Entity is null"),
            null,
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
        ));
    }

    [Test]
    public async Task DeleteAmenity_ReturnsTrue()
    {
        var amenity = amenities[0];
        var result = await _amenityService.DeleteAsync(amenity.Id);

        Assert.That(result, Is.True);
    }

    [Test]
    public void DeleteAmenityWithEmptyId_ThrowsArgumentNullException()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await _amenityService.DeleteAsync(Guid.Empty));

        _mockLogger.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((object v, Type _) => v.ToString() == "Entity ID is empty"),
            null,
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()
        ));
    }
}
