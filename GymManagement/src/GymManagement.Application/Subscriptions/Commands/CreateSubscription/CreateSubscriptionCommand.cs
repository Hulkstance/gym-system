using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.SubscriptionAggregate;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public record CreateSubscriptionCommand(SubscriptionType SubscriptionType, Guid AdminId) : IRequest<ErrorOr<Subscription>>;

public class CreateSubscriptionCommandHandler(IAdminsRepository adminsRepository) : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var admin = await adminsRepository.GetByIdAsync(command.AdminId);

        if (admin is null)
        {
            return Error.NotFound(description: "Admin not found");
        }

        if (admin.SubscriptionId is not null)
        {
            return Error.Conflict(description: "Admin already has active subscription");
        }

        var subscription = new Subscription(command.SubscriptionType, command.AdminId);
        admin.SetSubscription(subscription);

        await adminsRepository.UpdateAsync(admin);

        return subscription;
    }
}
