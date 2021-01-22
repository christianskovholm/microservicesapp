using Microsoft.EntityFrameworkCore;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;
using OrganizationService.Infrastructure.EntityTypeConfigurations;

namespace OrganizationService.Infrastructure
{
    public class OrganizationDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Role> Roles { get; set; }

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasSequence<int>("organizationseq").IncrementsBy(5);
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EventEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MemberEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationEntityTypeConfiguration());
        }
    }
}