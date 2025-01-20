using ErrorOr;

namespace GymManagement.Domain.Common.EventualConsistency;

public class EventualConsistencyException(Error eventualConsistencyError, List<Error>? underlyingErrors = null)
    : Exception(message: eventualConsistencyError.Description)
{
    public Error EventualConsistencyError { get; } = eventualConsistencyError;
    public List<Error> UnderlyingErrors { get; } = underlyingErrors ?? [];
}
