using ErrorOr;
using MediatR;
using SessionReservation.Application.Common.Interfaces;
using SessionReservation.Domain.RoomsAggregate;

namespace SessionReservation.Application.Rooms.Queries.GetRoom;

public record GetRoomQuery(Guid GymId, Guid RoomId) : IRequest<ErrorOr<Room>>;

public class GetRoomQueryHandler(IRoomsRepository roomsRepository) : IRequestHandler<GetRoomQuery, ErrorOr<Room>>
{
    public async Task<ErrorOr<Room>> Handle(GetRoomQuery query, CancellationToken cancellationToken) =>
        await roomsRepository.GetByIdAsync(query.RoomId) is not Room room
            ? Error.NotFound(description: "Room not found")
            : room;
}
