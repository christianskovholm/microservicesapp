using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    public class RoleEntityTypeConfiguration : DomainObjectEntityTypeConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasMaxLength(100);
        }
    }
}