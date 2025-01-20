using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.SubscriptionAggregate;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Persistence.Repositories;

public class SubscriptionsRepository(GymManagementDbContext dbContext) : ISubscriptionsRepository
{
    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        await dbContext.Subscriptions.AddAsync(subscription);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id) =>
        await dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(subscription => subscription.Id == id);

    public async Task<Subscription?> GetByIdAsync(Guid id) =>
        await dbContext.Subscriptions.FirstOrDefaultAsync(subscription => subscription.Id == id);

    public async Task<List<Subscription>> ListAsync() =>
        await dbContext.Subscriptions.ToListAsync();

    public async Task UpdateAsync(Subscription subscription)
    {
        dbContext.Update(subscription);
        await dbContext.SaveChangesAsync();
    }
}
