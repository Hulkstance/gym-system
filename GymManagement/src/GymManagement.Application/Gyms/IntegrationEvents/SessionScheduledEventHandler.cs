using GymManagement.Application.Common.Interfaces;
using MediatR;
using SharedKernel.IntegrationEvents;
using Throw;

namespace GymManagement.Application.Gyms.IntegrationEvents;

public class SessionScheduledEventHandler(IGymsRepository gymsRepository) : INotificationHandler<SessionScheduledIntegrationEvent>
{
    public async Task Handle(SessionScheduledIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(notification.RoomId);
        gym.ThrowIfNull();

        gym.AddTrainer(notification.TrainerId);
    }
}
