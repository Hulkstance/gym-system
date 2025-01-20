using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.RoomAggregate;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(Guid GymId, string RoomName) : IRequest<ErrorOr<Room>>;

public class CreateRoomCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository)
    : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        var subscription = await subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);

        if (subscription is null)
        {
            return Error.Unexpected(description: "Subscription not found");
        }

        var room = new Room(
            name: command.RoomName,
            gymId: gym.Id,
            maxDailySessions: subscription.GetMaxDailySessions());

        var addGymResult = gym.AddRoom(room);

        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        await gymsRepository.UpdateAsync(gym);

        return room;
    }
}
