using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.Common.EventualConsistency;
using SessionReservation.Domain.SessionAggregate.Events;

namespace SessionReservation.Application.Participants.Events;

public class ReservationCanceledEventHandler(IParticipantsRepository participantsRepository) : INotificationHandler<ReservationCanceledEvent>
{
    public async Task Handle(ReservationCanceledEvent notification, CancellationToken cancellationToken)
    {
        var participant = await participantsRepository.GetByIdAsync(notification.Reservation.ParticipantId)
            ?? throw new EventualConsistencyException(ReservationCanceledEvent.ParticipantNotFound);

        var removeBookingResult = participant.RemoveFromSchedule(notification.Session);

        if (removeBookingResult.IsError)
        {
            throw new EventualConsistencyException(
                ReservationCanceledEvent.ParticipantNotFound,
                removeBookingResult.Errors);
        }

        await participantsRepository.UpdateAsync(participant);
    }
}
