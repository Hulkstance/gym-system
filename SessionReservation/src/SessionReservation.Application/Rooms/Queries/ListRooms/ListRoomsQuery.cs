using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.RoomsAggregate;

namespace SessionReservation.Application.Rooms.Queries.ListRooms;

public record ListRoomsQuery(Guid GymId) : IRequest<ErrorOr<List<Room>>>;

public class ListRoomsQueryHandler(IRoomsRepository roomsRepository) : IRequestHandler<ListRoomsQuery, ErrorOr<List<Room>>>
{
    public async Task<ErrorOr<List<Room>>> Handle(ListRoomsQuery query, CancellationToken cancellationToken) =>
        await roomsRepository.ListByGymIdAsync(query.GymId);
}
