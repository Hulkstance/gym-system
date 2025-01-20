using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.GymAggregate;

using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym;

public record GetGymQuery(Guid SubscriptionId, Guid GymId) : IRequest<ErrorOr<Gym>>;

public class GetGymQueryHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        if (await subscriptionsRepository.ExistsAsync(request.SubscriptionId))
        {
            return Error.NotFound("Subscription not found");
        }

        if (await gymsRepository.GetByIdAsync(request.GymId) is not Gym gym)
        {
            return Error.NotFound(description: "GymManagement not found");
        }

        return gym;
    }
}
