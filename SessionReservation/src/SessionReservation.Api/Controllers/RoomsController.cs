using SessionReservation.Application.Rooms.Queries.GetRoom;
using SessionReservation.Application.Rooms.Queries.ListRooms;
using SessionReservation.Contracts.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SessionReservation.Api.Controllers;

[Route("gyms/{gymId:guid}/rooms")]
public class RoomsController(ISender sender) : ApiController
{
    [HttpGet("{roomId:guid}")]
    public async Task<IActionResult> GetRoom(Guid gymId, Guid roomId)
    {
        var query = new GetRoomQuery(gymId, roomId);

        var getRoomResult = await sender.Send(query);

        return getRoomResult.Match(
            room => Ok(new RoomResponse(room.Id, room.Name)),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> ListRooms(Guid gymId)
    {
        var query = new ListRoomsQuery(gymId);

        var listRoomsResult = await sender.Send(query);

        return listRoomsResult.Match(
            rooms => Ok(rooms.ConvertAll(room => new RoomResponse(room.Id, room.Name))),
            Problem);
    }
}
