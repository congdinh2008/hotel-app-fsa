using HotelApp.WebAPI.Models;

namespace HotelApp.WebAPI.Repositories;

/// <summary>
/// Interface for Room repository to handle CRUD operations for Room entities.
/// </summary>
public interface IRoomRepository
{
    /// <summary>
    /// Retrieves all Room entities.
    /// </summary>
    /// <returns>An enumerable collection of Room entities.</returns>
    IEnumerable<Room> GetAll();

    /// <summary>
    /// Retrieves a Room entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Room entity.</param>
    /// <returns>The Room entity if found; otherwise, null.</returns>
    Room? GetById(Guid id);

    /// <summary>
    /// Adds a new Room entity to the repository.
    /// </summary>
    /// <param name="entity">The Room entity to add.</param>
    /// <returns>The number of state entries written to the database.</returns>
    int Add(Room entity);

    /// <summary>
    /// Updates an existing Room entity in the repository.
    /// </summary>
    /// <param name="entity">The Room entity to update.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    bool Update(Room entity);

    /// <summary>
    /// Deletes a Room entity from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Room entity to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Deletes a Room entity from the repository.
    /// </summary>
    /// <param name="entity">The Room entity to delete.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    bool Delete(Room entity);
}