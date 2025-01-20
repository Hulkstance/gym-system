using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.Commands.AddTrainer;

public record AddTrainerCommand(Guid SubscriptionId, Guid GymId, Guid TrainerId) : IRequest<ErrorOr<Success>>;

public class AddTrainerCommandHandler(ISubscriptionsRepository subscriptionsRepository, IGymsRepository gymsRepository)
    : IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AddTrainerCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        if (!subscription.HasGym(command.GymId))
        {
            return Error.NotFound("GymManagement not found");
        }

        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound("GymManagement not found");
        }

        var addTrainerResult = gym.AddTrainer(command.TrainerId);

        if (addTrainerResult.IsError)
        {
            return addTrainerResult.Errors;
        }

        await gymsRepository.UpdateAsync(gym);

        return Result.Success;
    }
}
