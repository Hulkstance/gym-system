using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;

namespace SessionReservation.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand(Guid SessionId, Guid ParticipantId) : IRequest<ErrorOr<Success>>;

public class CreateReservationCommandHandler(ISessionsRepository sessionsRepository, IParticipantsRepository participantsRepository)
    : IRequestHandler<CreateReservationCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var session = await sessionsRepository.GetByIdAsync(command.SessionId);

        if (session is null)
        {
            return Error.NotFound(description: "Session not found");
        }

        if (session.HasReservationForParticipant(command.ParticipantId))
        {
            return Error.Conflict(description: "Participant already has reservation");
        }

        var participant = await participantsRepository.GetByIdAsync(command.ParticipantId);

        if (participant is null)
        {
            return Error.NotFound(description: "Participant not found");
        }

        if (participant.HasReservationForSession(session.Id))
        {
            return Error.Unexpected(description: "Participant not expected to have reservation to session");
        }

        if (!participant.IsTimeSlotFree(session.Date, session.Time))
        {
            return Error.Conflict(description: "Participant's calendar is not free for the entire session duration");
        }

        var reserveSpotResult = session.ReserveSpot(participant);

        if (reserveSpotResult.IsError)
        {
            return reserveSpotResult.Errors;
        }

        await sessionsRepository.UpdateAsync(session);

        return Result.Success;
    }
}
