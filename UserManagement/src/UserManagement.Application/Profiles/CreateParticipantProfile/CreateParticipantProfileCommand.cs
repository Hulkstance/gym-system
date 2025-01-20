using ErrorOr;
using MediatR;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.Profiles.CreateParticipantProfile;

public record CreateParticipantProfileCommand(Guid UserId) : IRequest<ErrorOr<Guid>>;

public class CreateParticipantProfileCommandHandler(IUsersRepository usersRepository) : IRequestHandler<CreateParticipantProfileCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateParticipantProfileCommand command, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(command.UserId);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        var createParticipantProfileResult = user.CreateParticipantProfile();

        await usersRepository.UpdateAsync(user);

        return createParticipantProfileResult;
    }
}
