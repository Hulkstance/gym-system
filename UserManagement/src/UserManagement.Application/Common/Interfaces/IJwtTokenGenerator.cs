using UserManagement.Domain.UserAggregate;

namespace UserManagement.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
