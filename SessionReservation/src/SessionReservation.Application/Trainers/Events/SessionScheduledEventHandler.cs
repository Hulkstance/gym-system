using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.Common.EventualConsistency;
using MediatR;
using SessionReservation.Domain.RoomsAggregate.Events;

namespace SessionReservation.Application.Trainers.Events;

public class SessionScheduledEventHandler(ITrainersRepository trainersRepository) : INotificationHandler<SessionScheduledEvent>
{
    public async Task Handle(SessionScheduledEvent notification, CancellationToken cancellationToken)
    {
        var trainer = await trainersRepository.GetByIdAsync(notification.Session.TrainerId)
            ?? throw new EventualConsistencyException(SessionScheduledEvent.TrainerNotFound);

        var updateTrainerScheduleResult = trainer.AddSessionToSchedule(notification.Session);

        if (updateTrainerScheduleResult.IsError)
        {
            throw new EventualConsistencyException(SessionScheduledEvent.TrainerScheduleUpdateFailed, updateTrainerScheduleResult.Errors);
        }

        await trainersRepository.UpdateAsync(trainer);
    }
}
