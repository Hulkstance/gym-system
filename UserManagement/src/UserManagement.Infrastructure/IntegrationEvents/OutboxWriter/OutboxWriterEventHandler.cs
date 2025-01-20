using System.Text.Json;
using MediatR;
using SharedKernel.IntegrationEvents;
using SharedKernel.IntegrationEvents.UserManagement;
using UserManagement.Domain.UserAggregate.Events;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.Infrastructure.IntegrationEvents.OutboxWriter;

public class OutboxWriterEventHandler(UserManagementDbContext dbContext) : INotificationHandler<AdminProfileCreatedEvent>,
    INotificationHandler<ParticipantProfileCreatedEvent>,
    INotificationHandler<TrainerProfileCreatedEvent>

{
    public async Task Handle(AdminProfileCreatedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new AdminProfileCreatedIntegrationEvent(
            notification.UserId,
            notification.AdminId);

        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(ParticipantProfileCreatedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new ParticipantProfileCreatedIntegrationEvent(notification.UserId, notification.ParticipantId);
        await AddOutboxIntegrationEventAsync(integrationEvent);
    }

    public async Task Handle(TrainerProfileCreatedEvent notification, CancellationToken cancellationToken)
    {
        var integrationEvent = new TrainerProfileCreatedIntegrationEvent(notification.UserId, notification.TrainerId);
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
