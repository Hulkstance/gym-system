using System.Text.Json;
using GymManagement.Domain.GymAggregate.Events;
using GymManagement.Infrastructure.Persistence;
using MediatR;
using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.GymManagement;

namespace GymManagement.Infrastructure.IntegrationEvents.OutboxWriter;

public class OutboxWriterEventHandler(GymManagementDbContext dbContext) : INotificationHandler<RoomAddedEvent>, INotificationHandler<RoomRemovedEvent>
{
    public async Task Handle(RoomAddedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new RoomAddedIntegrationEvent(
            Name: notification.Room.Name,
            RoomId: notification.Room.Id,
            GymId: notification.Gym.Id,
            MaxDailySessions: notification.Room.MaxDailySessions);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(RoomRemovedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new RoomRemovedIntegrationEvent(notification.RoomId);
        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    private async Task AddOutboxIntegrationEventAsync(IIntegrationEvent integrationEvent)
    {
        await dbContext.OutboxIntegrationEvents.AddAsync(new OutboxIntegrationEvent(
            integrationEvent.GetType().Name,
            JsonSerializer.Serialize(integrationEvent)));

        await dbContext.SaveChangesAsync();
    }
}
