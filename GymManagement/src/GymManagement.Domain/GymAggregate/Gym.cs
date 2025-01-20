using ErrorOr;
using GymManagement.Domain.Common;
using GymManagement.Domain.GymAggregate.Events;
using GymManagement.Domain.RoomAggregate;
using Throw;

namespace GymManagement.Domain.GymAggregate;

public class Gym : AggregateRoot
{
    private readonly List<Guid> _roomIds = [];
    private readonly List<Guid> _trainerIds = [];
    private readonly int _maxRooms;

    private Gym() { }

    public Gym(
        string name,
        int maxRooms,
        Guid subscriptionId,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        _maxRooms = maxRooms;
        SubscriptionId = subscriptionId;
    }

    public string Name { get; } = null!;

    public Guid SubscriptionId { get; }

    public IReadOnlyList<Guid> RoomIds => _roomIds;

    public ErrorOr<Success> AddRoom(Room room)
    {
        _roomIds.Throw().IfContains(room.Id);

        if (_roomIds.Count >= _maxRooms)
        {
            return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
        }

        _roomIds.Add(room.Id);

        _domainEvents.Add(new RoomAddedEvent(this, room));

        return Result.Success;
    }

    public bool HasRoom(Guid roomId) => _roomIds.Contains(roomId);

    public ErrorOr<Success> AddTrainer(Guid trainerId)
    {
        if (_trainerIds.Contains(trainerId))
        {
            return Error.Conflict(description: "Trainer already added to gym");
        }

        _trainerIds.Add(trainerId);
        return Result.Success;
    }

    public bool HasTrainer(Guid trainerId) => _trainerIds.Contains(trainerId);

    public void RemoveRoom(Guid roomId)
    {
        _roomIds.Remove(roomId);
        _domainEvents.Add(new RoomRemovedEvent(this, roomId));
    }
}
