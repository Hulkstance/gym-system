namespace GymManagement.Infrastructure.IntegrationEvents;

public class OutboxIntegrationEvent(string eventName, string eventContent)
{
    public Guid Id { get; } = Guid.NewGuid();
    public string EventName { get; } = eventName;
    public string EventContent { get; } = eventContent;
}
