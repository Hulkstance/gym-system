using UserManagement.Domain.UserAggregate;

namespace UserManagement.Application.Authentication.Common;

public record AuthenticationResult(User User, string Token);
