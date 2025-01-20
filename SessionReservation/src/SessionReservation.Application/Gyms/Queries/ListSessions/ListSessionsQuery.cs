using SessionReservation.Domain.SessionAggregate;
using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;

namespace SessionReservation.Application.Gyms.Queries.ListSessions;

public record ListSessionsQuery(
    Guid GymId,
    DateTime? StartDateTime = null,
    DateTime? EndDateTime = null,
    List<SessionCategory>? Categories = null) : IRequest<ErrorOr<List<Session>>>;

public class ListSessionsQueryHandler(ISessionsRepository sessionsRepository) : IRequestHandler<ListSessionsQuery, ErrorOr<List<Session>>>
{
    public async Task<ErrorOr<List<Session>>> Handle(ListSessionsQuery query, CancellationToken cancellationToken) =>
        await sessionsRepository.ListByGymIdAsync(query.GymId, query.StartDateTime, query.EndDateTime, query.Categories);
}
