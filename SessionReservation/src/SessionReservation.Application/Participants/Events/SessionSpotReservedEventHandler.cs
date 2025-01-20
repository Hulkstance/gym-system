using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.Common.EventualConsistency;
using SessionReservation.Domain.SessionAggregate.Events;
using Throw;

namespace SessionReservation.Application.Participants.Events;

public class SessionSpotReservedEventHandler(IParticipantsRepository participantsRepository) : INotificationHandler<SessionSpotReservedEvent>
{
    public async Task Handle(SessionSpotReservedEvent notification, CancellationToken cancellationToken)
    {
        var participant = await participantsRepository.GetByIdAsync(notification.Reservation.ParticipantId);
        participant.ThrowIfNull();

        var updateParticipantScheduleResult = participant.AddToSchedule(notification.Session);

        if (updateParticipantScheduleResult.IsError)
        {
            throw new EventualConsistencyException(
                SessionSpotReservedEvent.ParticipantScheduleUpdateFailed,
                updateParticipantScheduleResult.Errors);
        }

        await participantsRepository.UpdateAsync(participant);
    }
}
