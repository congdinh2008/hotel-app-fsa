using System;

namespace HotelApp.Business.Services.Base;

using System.Linq.Expressions;
using HotelApp.Business.ViewModels;

public interface IBaseService<T> where T : class
{

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of entities added.</returns>
    Task<int> AddAsync(T entity);

    /// <summary>
    /// Updates the specified entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a boolean value indicating whether the update was successful or not.</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>True if the entity was successfully deleted, otherwise false.</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Deletes an entity with the specified ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>A task representing the asynchronous operation. The task result is true if the entity was deleted successfully; otherwise, false.</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Deletes an entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>A task representing the asynchronous operation. The task result is a boolean value indicating whether the deletion was successful.</returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves a paginated result of entities based on the specified filter, ordering, and pagination parameters.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the entities.</param>
    /// <param name="orderBy">An optional ordering function to apply to the entities.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include in the result.</param>
    /// <param name="pageIndex">The index of the page to retrieve (1-based).</param>
    /// <param name="pageSize">The number of entities to include per page.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a paginated result of entities.</returns>
    Task<PaginatedResult<T>> GetAsync(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "", int pageIndex = 1, int pageSize = 10);
}
