using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Common;
using UserManagement.Domain.UserAggregate;
using UserManagement.Infrastructure.IntegrationEvents;
using UserManagement.Infrastructure.Middleware;

namespace UserManagement.Infrastructure.Persistence;

public class UserManagementDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<OutboxIntegrationEvent> OutboxIntegrationEvents => Set<OutboxIntegrationEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        var domainEventsQueue = httpContextAccessor.HttpContext!.Items.TryGetValue(EventualConsistencyMiddleware.DomainEventsKey, out var value) &&
                                value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new Queue<IDomainEvent>();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] = domainEventsQueue;

        return result;
    }
}
