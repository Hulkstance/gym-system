using GymManagement.Domain.Common;

namespace GymManagement.Domain.RoomAggregate;

public class Room : AggregateRoot
{
    public Room(string name, Guid gymId, int maxDailySessions, Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        Name = name;
        GymId = gymId;
        MaxDailySessions = maxDailySessions;
    }

    public string Name { get; }
    public Guid GymId { get; }
    public int MaxDailySessions { get; }
}
