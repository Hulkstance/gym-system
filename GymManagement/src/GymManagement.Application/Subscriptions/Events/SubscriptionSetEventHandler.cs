using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.AdminAggregate.Events;
using MediatR;

namespace GymManagement.Application.Subscriptions.Events;

public class SubscriptionSetEventHandler(ISubscriptionsRepository subscriptionsRepository) : INotificationHandler<SubscriptionSetEvent>
{
    public async Task Handle(SubscriptionSetEvent notification, CancellationToken cancellationToken) =>
        await subscriptionsRepository.AddSubscriptionAsync(notification.Subscription);
}
