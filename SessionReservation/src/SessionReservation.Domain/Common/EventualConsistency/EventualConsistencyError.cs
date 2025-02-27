using ErrorOr;

namespace SessionReservation.Domain.Common.EventualConsistency;

public static class EventualConsistencyError
{
    public const int EventualConsistencyType = 100;

    public static Error From(string code, string description) => Error.Custom(EventualConsistencyType, code, description);
}
