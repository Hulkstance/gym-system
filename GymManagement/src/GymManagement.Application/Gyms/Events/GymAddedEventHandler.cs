using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.SubscriptionAggregate.Events;
using MediatR;

namespace GymManagement.Application.Gyms.Events;

public class GymAddedEventHandler(IGymsRepository gymsRepository) : INotificationHandler<GymAddedEvent>
{
    public async Task Handle(GymAddedEvent notification, CancellationToken cancellationToken) =>
        await gymsRepository.AddGymAsync(notification.Gym);
}
