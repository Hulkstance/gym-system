using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.ParticipantAggregate;
using MediatR;
using SharedKernel.IntegrationEvents.UserManagement;

namespace SessionReservation.Application.Participants.IntegrationEvents;

public class ParticipantProfileCreatedEventHandler(IParticipantsRepository participantsRepository) : INotificationHandler<ParticipantProfileCreatedIntegrationEvent>
{
    public async Task Handle(ParticipantProfileCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var participant = new Participant(notification.UserId, id: notification.ParticipantId);

        await participantsRepository.AddParticipantAsync(participant);
    }
}
