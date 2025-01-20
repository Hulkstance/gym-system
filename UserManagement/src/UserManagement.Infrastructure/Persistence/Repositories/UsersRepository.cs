using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Domain.UserAggregate;

namespace UserManagement.Infrastructure.Persistence.Repositories;

public class UsersRepository(UserManagementDbContext dbContext) : IUsersRepository
{
    public async Task AddUserAsync(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email) =>
        await dbContext.Users.AnyAsync(user => user.Email == email);

    public async Task<User?> GetByEmailAsync(string email) =>
        await dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<User?> GetByIdAsync(Guid userId) =>
        await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);

    public async Task UpdateAsync(User user)
    {
        dbContext.Update(user);
        await dbContext.SaveChangesAsync();
    }
}
