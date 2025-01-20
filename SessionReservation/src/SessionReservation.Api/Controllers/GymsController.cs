using SessionReservation.Api.Controllers.Common;
using SessionReservation.Application.Gyms.Queries.ListSessions;
using SessionReservation.Contracts.Sessions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SessionReservation.Api.Controllers;

[Route("gyms")]
public class GymsController(ISender sender) : ApiController
{
    [HttpGet("{gymId:guid}/sessions")]
    public async Task<IActionResult> ListSessions(
        Guid gymId,
        DateTime? startDateTime = null,
        DateTime? endDateTime = null,
        [FromQuery] List<string>? categories = null)
    {
        var categoriesToDomainResult = SessionCategoryUtils.ToDomain(categories);

        if (categoriesToDomainResult.IsError)
        {
            return Problem(categoriesToDomainResult.Errors);
        }

        var command = new ListSessionsQuery(
            gymId,
            startDateTime,
            endDateTime,
            categoriesToDomainResult.Value);

        var listSessionsResult = await sender.Send(command);

        return listSessionsResult.Match(
            sessions => Ok(sessions.ConvertAll(
                session => new SessionResponse(
                    session.Id,
                    session.Name,
                    session.Description,
                    session.NumParticipants,
                    session.MaxParticipants,
                    session.Date.ToDateTime(session.Time.Start),
                    session.Date.ToDateTime(session.Time.End),
                    session.Categories.Select(category => category.Name).ToList()))),
            Problem);
    }
}
