using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Infrastructure.EntityTypeConfigurations
{
    /// <summary>
    /// Base EntityTypeConfiguration class for all classes deriving from DomainObject.
    /// </summary>
    public class DomainObjectEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : DomainObject
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Created)
                .HasColumnType("DateTimeOffset(0)");

            builder.Property(x => x.LastUpdated)
                .HasColumnType("DateTimeOffset(0)");
        }
    }
}