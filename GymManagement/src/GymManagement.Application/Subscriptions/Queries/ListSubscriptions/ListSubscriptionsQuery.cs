using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.SubscriptionAggregate;
using MediatR;

namespace GymManagement.Application.Subscriptions.Queries.ListSubscriptions;

// TODO: add admin id, for now, return all
public record ListSubscriptionsQuery : IRequest<ErrorOr<List<Subscription>>>;

public class ListSubscriptionsQueryHandler(ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<ListSubscriptionsQuery, ErrorOr<List<Subscription>>>
{
    public async Task<ErrorOr<List<Subscription>>> Handle(ListSubscriptionsQuery request, CancellationToken cancellationToken) =>
        await subscriptionsRepository.ListAsync();
}
