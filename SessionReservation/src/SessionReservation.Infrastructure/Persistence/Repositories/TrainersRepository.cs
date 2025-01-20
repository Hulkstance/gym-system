using Microsoft.EntityFrameworkCore;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.TrainerAggregate;

namespace SessionReservation.Infrastructure.Persistence.Repositories;

public class TrainersRepository(SessionReservationDbContext dbContext) : ITrainersRepository
{
    public async Task AddTrainerAsync(Trainer trainer)
    {
        await dbContext.Trainers.AddAsync(trainer);
        await dbContext.SaveChangesAsync();
    }

    public async Task<Trainer?> GetByIdAsync(Guid trainerId) =>
        await dbContext.Trainers.FirstOrDefaultAsync(trainer => trainer.Id == trainerId);

    public async Task UpdateAsync(Trainer trainer)
    {
        dbContext.Trainers.Update(trainer);
        await dbContext.SaveChangesAsync();
    }
}
