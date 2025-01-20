using Microsoft.EntityFrameworkCore;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.ParticipantAggregate;

namespace SessionReservation.Infrastructure.Persistence.Repositories;

public class ParticipantsRepository(SessionReservationDbContext dbContext) : IParticipantsRepository
{
    public async Task AddParticipantAsync(Participant participant)
    {
        await dbContext.Participants.AddAsync(participant);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Participant?> GetByIdAsync(Guid id) =>
        await dbContext.Participants.FirstOrDefaultAsync(participant => participant.Id == id);

    public async Task<List<Participant>> ListByIds(List<Guid> ids) =>
        await dbContext.Participants
            .Where(participant => ids.Contains(participant.Id))
            .ToListAsync();

    public async Task UpdateAsync(Participant participant)
    {
        dbContext.Update(participant);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(List<Participant> participants)
    {
        dbContext.UpdateRange(participants);
        await dbContext.SaveChangesAsync();
    }
}
