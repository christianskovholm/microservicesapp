using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    /// <summary>
    /// EntityTypeConfiguration for Event.
    /// </summary>
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("events");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.EventType)
                .HasColumnName("event_type")
                .IsRequired();

            builder.Property(x => x.Payload)
                .HasColumnName("payload")
                .IsRequired();

            builder.Property(x => x.Timestamp)
                .HasColumnName("timestamp")
                .HasColumnType("datetime2(0)")
                .IsRequired();

            builder.Property(x => x.OrganizationId)
                .HasColumnName("organization_id")
                .IsRequired();
        }
    }
}