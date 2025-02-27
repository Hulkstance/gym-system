using ErrorOr;
using SessionReservation.Domain.Common;
using SessionReservation.Domain.Common.Entities;
using SessionReservation.Domain.RoomsAggregate.Events;
using SessionReservation.Domain.SessionAggregate;

namespace SessionReservation.Domain.RoomsAggregate;

public class Room : AggregateRoot
{
    private readonly int _maxSessions;
    private readonly Schedule _schedule = Schedule.Empty();
    private readonly List<Guid> _sessionIds = [];

    private Room() { }

    public Room(
        string name,
        int maxDailySessions,
        Guid gymId,
        Schedule? schedule = null,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        _maxSessions = maxDailySessions;
        GymId = gymId;
        _schedule = schedule ?? Schedule.Empty();
    }

    public string Name { get; } = null!;
    public Guid GymId { get; }

    public IReadOnlyList<Guid> SessionIds => _sessionIds.AsReadOnly();

    public ErrorOr<Success> ScheduleSession(Session session)
    {
        if (SessionIds.Any(id => id == session.Id))
        {
            return Error.Conflict(description: "Session already exists in room");
        }

        if (_sessionIds.Count >= _maxSessions)
        {
            return RoomErrors.CannotHaveMoreSessionThanSubscriptionAllows;
        }

        var addEventResult = _schedule.BookTimeSlot(session.Date, session.Time);

        if (addEventResult.IsError && addEventResult.FirstError.Type == ErrorType.Conflict)
        {
            return RoomErrors.CannotHaveTwoOrMoreOverlappingSessions;
        }

        _sessionIds.Add(session.Id);
        _domainEvents.Add(new SessionScheduledEvent(this, session));

        return Result.Success;
    }

    public bool HasSession(Guid sessionId) => _sessionIds.Contains(sessionId);
}
