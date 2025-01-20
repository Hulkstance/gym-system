using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Infrastructure.IntegrationEvents;

namespace UserManagement.Infrastructure.Persistence.Configurations;

public class OutboxIntegrationEventConfigurations : IEntityTypeConfiguration<OutboxIntegrationEvent>
{
    public void Configure(EntityTypeBuilder<OutboxIntegrationEvent> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();

        builder.Property(o => o.EventName);
        builder.Property(o => o.EventContent);
    }
}
