using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.TrainerAggregate;
using MediatR;
using SharedKernel.IntegrationEvents.UserManagement;

namespace SessionReservation.Application.Trainers.IntegrationEvents;

public class TrainerProfileCreatedEventHandler(ITrainersRepository trainersRepository) : INotificationHandler<TrainerProfileCreatedIntegrationEvent>
{
    public async Task Handle(TrainerProfileCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var trainer = new Trainer(notification.UserId, id: notification.TrainerId);

        await trainersRepository.AddTrainerAsync(trainer);
    }
}
