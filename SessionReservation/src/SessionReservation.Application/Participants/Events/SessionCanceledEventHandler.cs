using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.Common.EventualConsistency;
using SessionReservation.Domain.SessionAggregate.Events;

namespace SessionReservation.Application.Participants.Events;

public class SessionCanceledEventHandler(IParticipantsRepository participantsRepository) : INotificationHandler<SessionCanceledEvent>
{
    public async Task Handle(SessionCanceledEvent notification, CancellationToken cancellationToken)
    {
        var participants = await participantsRepository.ListByIds(notification.Session.GetParticipantIds());

        participants.ForEach(participant =>
        {
            var removeFromScheduleResult = participant.RemoveFromSchedule(notification.Session);
            if (removeFromScheduleResult.IsError)
            {
                throw new EventualConsistencyException(
                    SessionCanceledEvent.ParticipantScheduleUpdateFailed,
                    removeFromScheduleResult.Errors);
            }
        });

        await participantsRepository.UpdateRangeAsync(participants);
    }
}
