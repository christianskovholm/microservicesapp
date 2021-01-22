using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OrganizationService.Infrastructure;

namespace OrganizationService.Application.Factories
{
    public class OrganizationDbContextFactory : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        /// <summary>
        /// Creates an OrganizationDbContext.
        /// </summary>
        /// <returns>An OrganizationDbContext.</returns>
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();

            optionsBuilder.UseSqlServer(ApplicationExtensions.GetSqlServerConnStr(), b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            
            return new OrganizationDbContext(optionsBuilder.Options);
        }
    }
}