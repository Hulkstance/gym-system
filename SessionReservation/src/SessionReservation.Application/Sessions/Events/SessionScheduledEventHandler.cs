using SessionReservation.Application.Common.Interfaces;
using MediatR;
using SessionReservation.Domain.RoomsAggregate.Events;

namespace SessionReservation.Application.Sessions.Events;

public class SessionScheduledEventHandler(ISessionsRepository sessionsRepository) : INotificationHandler<SessionScheduledEvent>
{
    public async Task Handle(SessionScheduledEvent notification, CancellationToken cancellationToken) =>
        await sessionsRepository.AddSessionAsync(notification.Session);
}
