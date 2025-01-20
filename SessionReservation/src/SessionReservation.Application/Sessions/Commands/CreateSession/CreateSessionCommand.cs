using SessionReservation.Domain.SessionAggregate;
using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.Common.ValueObjects;

namespace SessionReservation.Application.Sessions.Commands.CreateSession;

public record CreateSessionCommand(
    Guid RoomId,
    string Name,
    string Description,
    int MaxParticipants,
    DateTime StartDateTime,
    DateTime EndDateTime,
    Guid TrainerId,
    List<SessionCategory> Categories) : IRequest<ErrorOr<Session>>;

public class CreateSessionCommandHandler(ITrainersRepository trainersRepository, IRoomsRepository roomsRepository) : IRequestHandler<CreateSessionCommand, ErrorOr<Session>>
{
    public async Task<ErrorOr<Session>> Handle(CreateSessionCommand command, CancellationToken cancellationToken)
    {
        var room = await roomsRepository.GetByIdAsync(command.RoomId);

        if (room is null)
        {
            return Error.NotFound(description: "Room not found");
        }

        var trainer = await trainersRepository.GetByIdAsync(command.TrainerId);

        if (trainer is null)
        {
            return Error.NotFound(description: "Trainer not found");
        }

        var createTimeRangeResult = TimeRange.FromDateTimes(command.StartDateTime, command.EndDateTime);

        if (createTimeRangeResult.IsError && createTimeRangeResult.FirstError.Type == ErrorType.Validation)
        {
            return Error.Validation(description: "Invalid date and time");
        }

        if (!trainer.IsTimeSlotFree(DateOnly.FromDateTime(command.StartDateTime), createTimeRangeResult.Value))
        {
            return Error.Conflict(description: "Trainer's calendar is not free for the entire session duration");
        }

        var session = new Session(
            name: command.Name,
            description: command.Description,
            maxParticipants: command.MaxParticipants,
            roomId: command.RoomId,
            trainerId: command.TrainerId,
            date: DateOnly.FromDateTime(command.StartDateTime),
            time: createTimeRangeResult.Value,
            categories: command.Categories);

        var scheduleSessionResult = room.ScheduleSession(session);

        if (scheduleSessionResult.IsError)
        {
            return scheduleSessionResult.Errors;
        }

        await roomsRepository.UpdateAsync(room);

        return session;
    }
}
