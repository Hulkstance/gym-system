using SessionReservation.Application.Common.Interfaces;
using MediatR;
using SharedKernel.IntegrationEvents.GymManagement;

namespace SessionReservation.Application.Sessions.IntegrationEvents;

public class RoomRemovedEventHandler(ISessionsRepository sessionsRepository) : INotificationHandler<RoomRemovedIntegrationEvent>
{
    public async Task Handle(RoomRemovedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var sessions = await sessionsRepository.ListByRoomId(notification.RoomId);

        sessions.ForEach(session => session.Cancel());

        await sessionsRepository.RemoveRangeAsync(sessions);
    }
}
