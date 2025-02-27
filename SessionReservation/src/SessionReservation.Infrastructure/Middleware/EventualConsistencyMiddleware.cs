using MediatR;
using Microsoft.AspNetCore.Http;
using SessionReservation.Domain.Common;
using SessionReservation.Domain.Common.EventualConsistency;
using SessionReservation.Infrastructure.Persistence;

namespace SessionReservation.Infrastructure.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, SessionReservationDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (context.Items.TryGetValue(DomainEventsKey, out var value) && value is Queue<IDomainEvent> domainEvents)
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                await transaction.CommitAsync();
            }
            catch (EventualConsistencyException)
            {
                // handle eventual consistency exception
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        });

        await next(context);
    }
}
