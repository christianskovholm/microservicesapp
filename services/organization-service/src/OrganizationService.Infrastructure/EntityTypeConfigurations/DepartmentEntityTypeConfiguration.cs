using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    public class DepartmentEntityTypeConfiguration : DomainObjectEntityTypeConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}