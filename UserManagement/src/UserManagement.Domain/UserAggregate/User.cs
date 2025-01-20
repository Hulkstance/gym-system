using ErrorOr;
using UserManagement.Domain.Common;
using UserManagement.Domain.Common.Interfaces;
using UserManagement.Domain.UserAggregate.Events;

namespace UserManagement.Domain.UserAggregate;

public class User : AggregateRoot
{
    private readonly string _passwordHash = null!;

    private User() { }

    public User(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        Guid? adminId = null,
        Guid? participantId = null,
        Guid? trainerId = null,
        Guid? id = null)
        : base(id ?? Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AdminId = adminId;
        ParticipantId = participantId;
        TrainerId = trainerId;
        _passwordHash = passwordHash;
    }

    public string FirstName { get; } = null!;
    public string LastName { get; } = null!;
    public string Email { get; } = null!;
    public Guid? AdminId { get; private set; }
    public Guid? ParticipantId { get; private set; }
    public Guid? TrainerId { get; private set; }

    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher) =>
        passwordHasher.IsCorrectPassword(password, _passwordHash);

    public ErrorOr<Guid> CreateAdminProfile()
    {
        if (AdminId is not null)
        {
            return Error.Conflict(description: "User already has an admin profile");
        }

        AdminId = Guid.NewGuid();

        _domainEvents.Add(new AdminProfileCreatedEvent(Id, AdminId.Value));

        return AdminId.Value;
    }

    public ErrorOr<Guid> CreateParticipantProfile()
    {
        if (ParticipantId is not null)
        {
            return Error.Conflict(description: "User already has a participant profile");
        }

        ParticipantId = Guid.NewGuid();

        _domainEvents.Add(new ParticipantProfileCreatedEvent(Id, ParticipantId.Value));

        return ParticipantId.Value;
    }

    public ErrorOr<Guid> CreateTrainerProfile()
    {
        if (TrainerId is not null)
        {
            return Error.Conflict(description: "User already has a trainer profile");
        }

        TrainerId = Guid.NewGuid();

        _domainEvents.Add(new TrainerProfileCreatedEvent(Id, TrainerId.Value));

        return TrainerId.Value;
    }
}
