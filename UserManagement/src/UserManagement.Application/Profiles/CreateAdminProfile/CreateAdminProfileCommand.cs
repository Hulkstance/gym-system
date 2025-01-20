using ErrorOr;
using MediatR;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.Profiles.CreateAdminProfile;

public record CreateAdminProfileCommand(Guid UserId) : IRequest<ErrorOr<Guid>>;

public class CreateAdminProfileCommandHandler(IUsersRepository usersRepository) : IRequestHandler<CreateAdminProfileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateAdminProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        var createAdminProfileResult = user.CreateAdminProfile();

        await usersRepository.UpdateAsync(user);

        return createAdminProfileResult;
    }
}
