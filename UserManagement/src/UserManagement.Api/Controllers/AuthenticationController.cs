using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Authentication.Common;
using UserManagement.Application.Authentication.Login;
using UserManagement.Application.Authentication.Register;
using UserManagement.Contracts.Authentication;

namespace UserManagement.Api.Controllers;

[Route("[controller]")]
[AllowAnonymous]
public class AuthenticationController(ISender mediator) : ApiController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
        var authResult = await mediator.Send(command);

        return authResult.Match(
            result => base.Ok(MapToAuthResponse(result)),
            Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var query = new LoginQuery(request.Email, request.Password);
        var authResult = await mediator.Send(query);

        if (authResult.IsError && authResult.FirstError == AuthenticationErrors.InvalidCredentials)
        {
            return Problem(
                detail: authResult.FirstError.Description,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        return authResult.Match(
            result => Ok(MapToAuthResponse(result)),
            Problem);
    }

    private static AuthenticationResponse MapToAuthResponse(AuthenticationResult authResult) =>
        new(
            authResult.User.Id,
            authResult.User.FirstName,
            authResult.User.LastName,
            authResult.User.Email,
            authResult.Token);
}
