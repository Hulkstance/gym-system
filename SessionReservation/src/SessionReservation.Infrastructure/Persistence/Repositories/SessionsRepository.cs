using Microsoft.EntityFrameworkCore;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.SessionAggregate;

namespace SessionReservation.Infrastructure.Persistence.Repositories;

public class SessionsRepository(SessionReservationDbContext dbContext) : ISessionsRepository
{
    public async Task AddSessionAsync(Session session)
    {
        await dbContext.Sessions.AddAsync(session);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Session?> GetByIdAsync(Guid id) =>
        await dbContext.Sessions.FirstOrDefaultAsync(session => session.Id == id);

    public async Task<List<Session>> ListByIds(
        IReadOnlyList<Guid> sessionIds,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null,
        List<SessionCategory>? categories = null) =>
        await dbContext.Sessions
            .AsNoTracking()
            .Where(session => sessionIds.Contains(session.Id))
            .WhereBetweenDateAndTimes(startDateTime, endDateTime)
            .WhereOfCategory(categories)
            .ToListAsync();

    public async Task<List<Session>> ListByGymIdAsync(
        Guid gymId,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null,
        List<SessionCategory>? categories = null)
    {
        var gymRooms = await dbContext.Rooms
            .AsNoTracking()
            .Where(room => room.GymId == gymId)
            .ToListAsync();

        var sessionIds = gymRooms.SelectMany(room => room.SessionIds).ToList();

        return await ListByIds(sessionIds, startDateTime, endDateTime, categories);
    }

    public async Task UpdateAsync(Session session)
    {
        dbContext.Update(session);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<Session>> ListByRoomId(Guid roomId) =>
        await dbContext.Sessions
            .Where(session => session.RoomId == roomId)
            .ToListAsync();

    public async Task RemoveRangeAsync(List<Session> sessions)
    {
        dbContext.RemoveRange(sessions);
        await dbContext.SaveChangesAsync();
    }
}

file static class DbContextSessionExtensions
{
    public static IQueryable<Session> WhereBetweenDateAndTimes(this IQueryable<Session> query, DateTime? start, DateTime? end)
    {
        if (start == null && end == null)
        {
            return query;
        }

        start ??= DateTime.MinValue;
        end ??= DateTime.MaxValue;

        return query
            .AsNoTracking()
            .Where(session => session.Date >= DateOnly.FromDateTime(start.Value))
            .Where(session => session.Date <= DateOnly.FromDateTime(end.Value))
            .Where(session => session.Time.Start >= TimeOnly.FromDateTime(start.Value));
    }

    public static IQueryable<Session> WhereOfCategory(this IQueryable<Session> query, List<SessionCategory>? categories)
    {
        if (categories is null || categories.Count == 0)
        {
            return query;
        }

        var categoryNames = categories.ConvertAll(category => category.Name);

        return query
            .AsNoTracking()
            .Where(session => session.Categories.Any(category => categoryNames.Contains(category.Name)));
    }
}
