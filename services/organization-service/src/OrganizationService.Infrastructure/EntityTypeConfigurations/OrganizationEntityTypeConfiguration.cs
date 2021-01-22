using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    public class OrganizationEntityTypeConfiguration : DomainObjectEntityTypeConfiguration<Organization>
    {
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(200);
        }
    }
}