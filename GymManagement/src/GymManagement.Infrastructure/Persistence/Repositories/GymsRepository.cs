using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.GymAggregate;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Persistence.Repositories;

public class GymsRepository(GymManagementDbContext dbContext) : IGymsRepository
{
    public async Task AddGymAsync(Gym gym)
    {
        await dbContext.Gyms.AddAsync(gym);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await dbContext.Gyms.AsNoTracking().AnyAsync(gym => gym.Id == id);

    public async Task<Gym?> GetByIdAsync(Guid id) =>
        await dbContext.Gyms.FirstOrDefaultAsync(gym => gym.Id == id);

    public async Task<List<Gym>> ListSubscriptionGyms(Guid subscriptionId) =>
        await dbContext.Gyms
            .Where(gym => gym.SubscriptionId == subscriptionId)
            .ToListAsync();

    public async Task UpdateAsync(Gym gym)
    {
        dbContext.Update(gym);
        await dbContext.SaveChangesAsync();
    }
}
