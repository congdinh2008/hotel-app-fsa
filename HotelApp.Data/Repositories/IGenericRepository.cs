using System.Linq.Expressions;

namespace HotelApp.Data.Repositories;

/// <summary>
/// Represents a generic repository of entities.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities of type T.
    /// </summary>
    /// <returns>An enumerable collection of entities.</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity with the specified identifier, or null if not found.</returns>
    T? GetById(Guid id);

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    void Add(T entity);

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    void Delete(Guid id);

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> representing the query for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <returns>An <see cref="IQueryable{T}"/> representing the query for the specified entity type.</returns>
    IQueryable<T> GetQuery();

    /// <summary>
    /// Retrieves a queryable collection of entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate used to filter the entities.</param>
    /// <returns>A queryable collection of entities that satisfy the specified predicate.</returns>
    IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Retrieves a collection of entities from the repository based on the specified filter, order, and inclusion criteria.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the entities.</param>
    /// <param name="orderBy">An optional function to specify the order in which the entities should be returned.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include in the query results.</param>
    /// <returns>An <see cref="IQueryable{T}"/> representing the collection of entities that match the specified criteria.</returns>
    IQueryable<T> Get(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "");
}