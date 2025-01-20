using ErrorOr;
using MediatR;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Application.Profiles.ListProfiles;

public record ListProfilesQuery(Guid UserId) : IRequest<ErrorOr<ListProfilesResult>>;

public class ListProfilesQueryHandler(IUsersRepository usersRepository) : IRequestHandler<ListProfilesQuery, ErrorOr<ListProfilesResult>>
{
    public async Task<ErrorOr<ListProfilesResult>> Handle(ListProfilesQuery query, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetByIdAsync(query.UserId);

        if (user is null)
        {
            return Error.NotFound(description: "User not found");
        }

        return new ListProfilesResult(user.AdminId, user.ParticipantId, user.TrainerId);
    }
}
