using SessionReservation.Domain.SessionAggregate;
using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;

namespace SessionReservation.Application.Sessions.Queries.GetSession;

public record GetSessionQuery(Guid RoomId, Guid SessionId) : IRequest<ErrorOr<Session>>;

public class GetSessionQueryHandler(ISessionsRepository sessionsRepository, IRoomsRepository roomsRepository) : IRequestHandler<GetSessionQuery, ErrorOr<Session>>
{
    public async Task<ErrorOr<Session>> Handle(GetSessionQuery query, CancellationToken cancellationToken)
    {
        var room = await roomsRepository.GetByIdAsync(query.RoomId);

        if (room is null)
        {
            return Error.NotFound(description: "Room not found");
        }

        if (!room.HasSession(query.SessionId))
        {
            return Error.NotFound(description: "Session not found");
        }

        if (await sessionsRepository.GetByIdAsync(query.SessionId) is not Session session)
        {
            return Error.NotFound(description: "Session not found");
        }

        return session;
    }
}
