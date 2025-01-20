using SessionReservation.Domain.Common;

namespace SessionReservation.Domain.SessionAggregate;

public class Reservation : Entity
{
    public Reservation(Guid participantId, Guid? id = null)
        : base(id ?? Guid.NewGuid()) =>
        ParticipantId = participantId;

    private Reservation() { }

    public Guid ParticipantId { get; }
}
