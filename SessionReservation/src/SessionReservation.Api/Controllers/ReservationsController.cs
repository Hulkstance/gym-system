using SessionReservation.Application.Reservations.Commands.CreateReservation;
using SessionReservation.Contracts.Reservations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SessionReservation.Api.Controllers;

[Route("sessions/{sessionId:guid}/reservations")]
public class ReservationsController(ISender sender) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateReservation(CreateReservationRequest request, Guid sessionId)
    {
        var command = new CreateReservationCommand(
            sessionId,
            request.ParticipantId);

        var createReservationResult = await sender.Send(command);

        return createReservationResult.Match(_ => NoContent(), Problem);
    }
}
