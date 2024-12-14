using HotelApp.Data.Contexts;
using HotelApp.Data.Models;
using HotelApp.Data.Repositories;

namespace HotelApp.Data.UnitOfWork;

public class UnitOfWork(HotelAppDbContext context) : IUnitOfWork
{
    private readonly HotelAppDbContext _context = context;

    private IGenericRepository<Room>? _roomRepository;

    public IGenericRepository<Room> RoomRepository => _roomRepository ??= new GenericRepository<Room>(_context);

    private IGenericRepository<Amenity>? _amenityRepository;

    public IGenericRepository<Amenity> AmenityRepository => _amenityRepository ??= new GenericRepository<Amenity>(_context);

    private IGenericRepository<Booking>? _bookingRepository;

    public IGenericRepository<Booking> BookingRepository => _bookingRepository ??= new GenericRepository<Booking>(_context);

    private IGenericRepository<BookingDetail>? _bookingDetailRepository;

    public IGenericRepository<BookingDetail> BookingDetailRepository => _bookingDetailRepository ??= new GenericRepository<BookingDetail>(_context);

    private IGenericRepository<BookingAmenity>? _bookingAmenityRepository;

    public IGenericRepository<BookingAmenity> BookingAmenityRepository => _bookingAmenityRepository ??= new GenericRepository<BookingAmenity>(_context);

    private IGenericRepository<User>? _userRepository;

    public IGenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);

    private IGenericRepository<Role>? _roleRepository;

    public IGenericRepository<Role> RoleRepository => _roleRepository ??= new GenericRepository<Role>(_context);

    public HotelAppDbContext Context => _context;

    public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : class => new GenericRepository<TEntity>(_context);

    /// <summary>
    /// Saves all changes made in the unit of work to the underlying data store.
    /// </summary>
    /// <returns>The number of objects written to the underlying data store.</returns>
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    /// <summary>
    /// Saves all changes made in this unit of work to the underlying database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Begins a new transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Commits the current transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>

    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    /// <summary>
    /// Rolls back the current transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~UnitOfWork()
    {
        Dispose(false);
    }
}
