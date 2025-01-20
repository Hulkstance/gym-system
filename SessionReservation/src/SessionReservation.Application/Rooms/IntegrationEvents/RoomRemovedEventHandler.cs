using SessionReservation.Application.Common.Interfaces;
using MediatR;
using SharedKernel.IntegrationEvents.GymManagement;

namespace SessionReservation.Application.Rooms.IntegrationEvents;

public class RoomRemovedEventHandler(IRoomsRepository roomsRepository) : INotificationHandler<RoomRemovedIntegrationEvent>
{
    public async Task Handle(RoomRemovedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var room = await roomsRepository.GetByIdAsync(notification.RoomId);

        if (room is not null)
        {
            await roomsRepository.RemoveAsync(room);
        }
    }
}
