
using System.Linq.Expressions;
using HotelApp.Business.ViewModels;
using HotelApp.Data.UnitOfWork;

namespace HotelApp.Business.Services.Base;
public class BaseService<T>(IUnitOfWork unitOfWork) : IBaseService<T> where T : class
{
    protected readonly IUnitOfWork _unitOfWork = unitOfWork;

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of entities added.</returns>
    public virtual int Add(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _unitOfWork.GenericRepository<T>().Add(entity);
        return _unitOfWork.SaveChanges();
    }

    /// <summary>
    /// Updates the specified entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the update was successful or not.</returns>
    public virtual async Task<int> AddAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _unitOfWork.GenericRepository<T>().Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new InvalidOperationException("Entity not added");
        }

        return result;
    }

    /// <summary>
    /// Updates the specified entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the update was successful or not.</returns>
    public virtual async Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _unitOfWork.GenericRepository<T>().Update(entity);
        var result = await _unitOfWork.SaveChangesAsync() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Entity not updated");
        }

        return result;
    }

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted, otherwise false.</returns>
    public virtual bool Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        _unitOfWork.GenericRepository<T>().Delete(id);
        var result = _unitOfWork.SaveChanges() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Entity not deleted");
        }

        return result;
    }

    /// <summary>
    /// Deletes an entity with the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result is true if the entity was deleted successfully; otherwise, false.</returns>
    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        _unitOfWork.GenericRepository<T>().Delete(id);
        var result = await _unitOfWork.SaveChangesAsync() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Entity not deleted");
        }

        return result;
    }

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether the deletion was successful.</returns>
    public virtual async Task<bool> DeleteAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _unitOfWork.GenericRepository<T>().Delete(entity);
        var result = await _unitOfWork.SaveChangesAsync() > 0;

        if (!result)
        {
            throw new InvalidOperationException("Entity not deleted");
        }

        return result;
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.GenericRepository<T>().GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _unitOfWork.GenericRepository<T>().GetAllAsync();
    }

    /// <summary>
    /// Retrieves a paginated result of entities based on the specified filter, ordering, and pagination parameters.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the entities.</param>
    /// <param name="orderBy">An optional ordering function to apply to the entities.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include in the result.</param>
    /// <param name="pageIndex">The index of the page to retrieve (1-based).</param>
    /// <param name="pageSize">The number of entities to include per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of entities.</returns>
    public virtual async Task<PaginatedResult<T>> GetAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "", int pageIndex = 1, int pageSize = 10)
    {
        var query = _unitOfWork.GenericRepository<T>().Get(filter, orderBy, includeProperties);

        return await PaginatedResult<T>.CreateAsync(query, pageIndex, pageSize);
    }
}