using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.AdminAggregate;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Persistence.Repositories;

public class AdminsRepository(GymManagementDbContext dbContext) : IAdminsRepository
{
    public async Task AddAdminAsync(Admin admin)
    {
        await dbContext.Admins.AddAsync(admin);
        await dbContext.SaveChangesAsync();
    }

    public Task<Admin?> GetByIdAsync(Guid adminId) =>
        dbContext.Admins.FirstOrDefaultAsync(admin => admin.Id == adminId);

    public async Task UpdateAsync(Admin admin)
    {
        dbContext.Update(admin);
        await dbContext.SaveChangesAsync();
    }
}
