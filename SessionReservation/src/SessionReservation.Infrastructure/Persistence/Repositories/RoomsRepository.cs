using Microsoft.EntityFrameworkCore;

using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.RoomsAggregate;

namespace SessionReservation.Infrastructure.Persistence.Repositories;

public class RoomsRepository(SessionReservationDbContext dbContext) : IRoomsRepository
{
    public async Task AddRoomAsync(Room room)
    {
        await dbContext.Rooms.AddAsync(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Room?> GetByIdAsync(Guid id)
    {
        return await dbContext.Rooms.FirstOrDefaultAsync(room => room.Id == id);
    }

    public async Task<List<Room>> ListByGymIdAsync(Guid gymId)
    {
        return await dbContext.Rooms
            .AsNoTracking()
            .Where(room => room.GymId == gymId)
            .ToListAsync();
    }

    public async Task RemoveAsync(Room room)
    {
        dbContext.Rooms.Remove(room);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Room room)
    {
        dbContext.Rooms.Update(room);
        await dbContext.SaveChangesAsync();
    }
}
