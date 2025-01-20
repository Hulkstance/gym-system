namespace GymManagement.Domain.Common;

public abstract class Entity
{
    protected Entity(Guid id) => Id = id;

    protected Entity() { }

    public Guid Id { get; init; }

    public override bool Equals(object? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return ((Entity)other).Id == Id;
    }

    public override int GetHashCode() => Id.GetHashCode();
}
