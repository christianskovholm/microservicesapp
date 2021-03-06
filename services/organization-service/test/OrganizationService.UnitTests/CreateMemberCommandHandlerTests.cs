using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OrganizationService.Application.Commands;
using OrganizationService.Domain.Aggregates.Organization;
using Xunit;

namespace OrganizationService.UnitTests
{
    public class CreateMemberCommandHandlerTests : CommandHandlerTestBase
    {
        [Fact]
        public async void Handle_Should_Create_And_Return_Member()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var role = organization.CreateRole("test");
            var department = organization.CreateDepartment("test");

            OrganizationRepositoryMock.Setup(x => x.GetAsync(organization.Id)).Returns(Task.FromResult(organization));

            var command = new CreateMemberCommand("test", role.Id, organization.Id, department.Id);
            var handler = new CreateMemberCommandHandler(OrganizationRepositoryMock.Object);

            // Act
            await handler.Handle(command, new CancellationToken());

            // Assert
            organization.Departments.Single().Members.Single().Name.Should().Be(command.Name);
            OrganizationRepositoryMock.Verify(x => x.Update(organization), Times.Once());
        }
    }
}