using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    public class MemberEntityTypeConfiguration : DomainObjectEntityTypeConfiguration<Member>
    {
        public override void Configure(EntityTypeBuilder<Member> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasMaxLength(150);
        }
    }
}