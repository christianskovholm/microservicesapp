using System.Collections;
using FluentAssertions;
using OrganizationService.Application;
using OrganizationService.Application.Factories;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure;
using OrganizationService.Infrastructure.Queries;
using Xunit;

namespace OrganizationService.IntegrationTests
{
    public class OrganizationQueriesTests
    {
        private readonly IQueries _queries;
        private readonly OrganizationDbContext _context;

        public OrganizationQueriesTests()
        {
            _queries = new Queries(ApplicationExtensions.GetSqlServerConnStr());
            _context = new OrganizationDbContextFactory().CreateDbContext(null);
        }

        [Fact]
        public async void GetOrganizationAsync_Should_Return_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var role = organization.CreateRole("test");
            var department = organization.CreateDepartment("test");

            _context.Add(organization);
            await _context.SaveChangesAsync();

            // Act
            var serializedOrganization = await _queries.GetOrganizationAsync(organization.Id);

            // Assert
            serializedOrganization.Value<string>("name").Should().Be(organization.Name);
            serializedOrganization.Value<string>("description").Should().Be(organization.Description);
            serializedOrganization.Value<IEnumerable>("departments").Should().NotBeNull();
        }
    }
}