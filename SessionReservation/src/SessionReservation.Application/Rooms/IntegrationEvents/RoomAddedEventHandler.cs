using SessionReservation.Application.Common.Interfaces;
using MediatR;
using SessionReservation.Domain.RoomsAggregate;
using SharedKernel.IntegrationEvents.GymManagement;

namespace SessionReservation.Application.Rooms.IntegrationEvents;

public class RoomAddedEventHandler(IRoomsRepository roomsRepository) : INotificationHandler<RoomAddedIntegrationEvent>
{
    public async Task Handle(RoomAddedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var room = new Room(
            notification.Name,
            notification.MaxDailySessions,
            notification.GymId,
            id: notification.RoomId);

        await roomsRepository.AddRoomAsync(room);
    }
}
