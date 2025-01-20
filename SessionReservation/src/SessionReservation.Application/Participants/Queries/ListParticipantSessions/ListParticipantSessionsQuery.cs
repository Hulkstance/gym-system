using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.SessionAggregate;

namespace SessionReservation.Application.Participants.Queries.ListParticipantSessions;

public record ListParticipantSessionsQuery(Guid ParticipantId, DateTime? StartDateTime = null, DateTime? EndDateTime = null) : IRequest<ErrorOr<List<Session>>>;

public class ListParticipantSessionsQueryHandler(ISessionsRepository sessionsRepository, IParticipantsRepository participantsRepository)
    : IRequestHandler<ListParticipantSessionsQuery, ErrorOr<List<Session>>>
{
    public async Task<ErrorOr<List<Session>>> Handle(ListParticipantSessionsQuery query, CancellationToken cancellationToken)
    {
        var participant = await participantsRepository.GetByIdAsync(query.ParticipantId);

        if (participant is null)
        {
            return Error.NotFound(description: "Participant not found");
        }

        return await sessionsRepository.ListByIds(participant.SessionIds, query.StartDateTime, query.EndDateTime);
    }
}
