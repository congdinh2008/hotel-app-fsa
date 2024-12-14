using HotelApp.Data.Contexts;
using HotelApp.Data.Models;

namespace HotelApp.Data.Repositories;

public class RoomRepository(HotelAppDbContext context) : IRoomRepository
{
    private readonly HotelAppDbContext _context = context;

    public IEnumerable<Room> GetAll()
    {
        return [.. _context.Rooms];
    }

    public Room? GetById(Guid id)
    {
        return _context.Rooms.Find(id);
    }

    public int Add(Room entity)
    {
        _context.Rooms.Add(entity);
        return _context.SaveChanges();
    }

    public bool Update(Room entity)
    {
        _context.Rooms.Update(entity);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(Guid id)
    {
        var room = _context.Rooms.Find(id);
        if (room == null)
        {
            return false;
        }
        _context.Rooms.Remove(room);
        return _context.SaveChanges() > 0;
    }

    public bool Delete(Room entity)
    {
        _context.Rooms.Remove(entity);
        return _context.SaveChanges() > 0;
    }
}
