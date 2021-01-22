using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using OrganizationService.Application.Commands;
using OrganizationService.Domain.Aggregates.Organization;
using Xunit;

namespace OrganizationService.FunctionalTests
{
    public class OrganizationServiceFunctionalTests
    {
        private readonly TestServer _testServer;

        public OrganizationServiceFunctionalTests()
        {
            _testServer = FunctionalTestExtensions.CreateTestServer();
        }

        [Fact]
        public async void Get_Should_Return_Ok()
        {
            // Arrange
            var organization = new Organization("test", "test");
            _testServer.CreateOrganization(organization);

            // Act
            var response = await _testServer.CreateClient().GetAsync($"/organizations/{organization.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async void Post_Should_Create_Organization_And_Return_Ok()
        {
            // Arrange
            var command = new CreateOrganizationCommand { Name = "test", Description = "test" };

            // Act
            var response = await _testServer.CreateClient().PostAsync("/organizations", command.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateDepartment_Should_Create_Department_And_Return_Ok()
        {
            // Arrange
            var command = new CreateDepartmentCommand { Name = "test" };
            var organization = new Organization("test", "test");
            _testServer.CreateOrganization(organization);

            // Act
            var response = await _testServer.CreateClient().PostAsync($"/organizations/{organization.Id}/departments", command.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateRole_Should_Create_Role_And_Return_Ok()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var command = new CreateRoleCommand { RoleName = "test" };
            _testServer.CreateOrganization(organization);

            // Act
            var response = await _testServer.CreateClient().PostAsync($"/organizations/{organization.Id}/roles", command.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateMember_Should_Create_Member_And_Return_Ok()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var role = organization.CreateRole("test");
            var department = organization.CreateDepartment("test");
            _testServer.CreateOrganization(organization);
            var command = new CreateMemberCommand { Name = "test", RoleId = role.Id };

            // Act
            var response = await _testServer.CreateClient().PostAsync($"/organizations/{organization.Id}/departments/{department.Id}/members", command.ToStringContent());

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}