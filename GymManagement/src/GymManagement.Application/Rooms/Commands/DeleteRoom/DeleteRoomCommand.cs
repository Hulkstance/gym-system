using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(Guid GymId, Guid RoomId) : IRequest<ErrorOr<Deleted>>;

public class DeleteRoomCommandHandler(IGymsRepository gymsRepository) : IRequestHandler<DeleteRoomCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        if (!gym.HasRoom(command.RoomId))
        {
            return Error.NotFound(description: "Room not found");
        }

        gym.RemoveRoom(command.RoomId);

        await gymsRepository.UpdateAsync(gym);

        return Result.Deleted;
    }
}
