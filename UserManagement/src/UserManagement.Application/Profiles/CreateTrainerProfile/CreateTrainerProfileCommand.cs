using ErrorOr;
using MediatR;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.Profiles.CreateTrainerProfile;

public record CreateTrainerProfileCommand(Guid UserId) : IRequest<ErrorOr<Guid>>;

public class CreateTrainerProfileCommandHandler(IUsersRepository usersRepository) : IRequestHandler<CreateTrainerProfileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateTrainerProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        var createTrainerProfileResult = user.CreateTrainerProfile();

        await usersRepository.UpdateAsync(user);

        return createTrainerProfileResult;
    }
}
