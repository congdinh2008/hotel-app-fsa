using System.Linq.Expressions;
using HotelApp.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace HotelApp.Data.Repositories;

/// <summary>
/// Represents a generic repository of entities.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public readonly HotelAppDbContext Context;

    private readonly DbSet<T> _dbSet;

    public GenericRepository(HotelAppDbContext context)
    {
        Context = context;
        _dbSet = Context.Set<T>();
    }

    /// <summary>
    /// Retrieves all entities of type T.
    /// </summary>
    /// <returns>An enumerable collection of entities.</returns>
    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the collection of entities.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity with the specified identifier, or null if not found.</returns>
    public T? GetById(Guid id)
    {
        return _dbSet.Find(id);
    }

    /// <summary>
    /// Retrieves an entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier.</returns>
    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    /// <summary>
    /// Updates an entity.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>

    public void Delete(Guid id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
        }
    }

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    /// <summary>
    /// Retrieves an <see cref="IQueryable{T}"/> representing the query for the specified entity type.
    /// </summary>
    /// <typeparam name="T">The type of entity.</typeparam>
    /// <returns>An <see cref="IQueryable{T}"/> representing the query for the specified entity type.</returns>
    public IQueryable<T> GetQuery()
    {
        return _dbSet;
    }

    /// <summary>
    /// Retrieves a queryable collection of entities that satisfy the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate used to filter the entities.</param>
    /// <returns>A queryable collection of entities that satisfy the specified predicate.</returns>
    public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    /// <summary>
    /// Retrieves a collection of entities from the database based on the specified filter, order, and included properties.
    /// </summary>
    /// <param name="filter">An optional filter expression to apply to the query.</param>
    /// <param name="orderBy">An optional function to order the query results.</param>
    /// <param name="includeProperties">A comma-separated list of navigation properties to include in the query results.</param>
    /// <returns>An <see cref="IQueryable{T}"/> representing the collection of entities.</returns>
    public virtual IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrWhiteSpace(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        return orderBy != null ? orderBy(query) : query;
    }
}