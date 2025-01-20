using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.AdminAggregate;
using MediatR;
using SharedKernel.IntegrationEvents.UserManagement;

namespace GymManagement.Application.Admins.IntegrationEvents;

public class AdminProfileCreatedEventHandler(IAdminsRepository adminsRepository) : INotificationHandler<AdminProfileCreatedIntegrationEvent>
{
    public async Task Handle(AdminProfileCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var admin = new Admin(notification.UserId, id: notification.AdminId);

        await adminsRepository.AddAdminAsync(admin);
    }
}
