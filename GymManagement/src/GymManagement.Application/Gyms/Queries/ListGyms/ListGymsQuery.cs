using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.GymAggregate;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.ListGyms;

public record ListGymsQuery(Guid SubscriptionId) : IRequest<ErrorOr<List<Gym>>>;

public class ListGymsQueryHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<ListGymsQuery, ErrorOr<List<Gym>>>
{
    public async Task<ErrorOr<List<Gym>>> Handle(ListGymsQuery query, CancellationToken cancellationToken)
    {
        if (!await subscriptionsRepository.ExistsAsync(query.SubscriptionId))
        {
            return Error.NotFound(description: "Subscription not found");
        }

        return await gymsRepository.ListSubscriptionGyms(query.SubscriptionId);
    }
}
