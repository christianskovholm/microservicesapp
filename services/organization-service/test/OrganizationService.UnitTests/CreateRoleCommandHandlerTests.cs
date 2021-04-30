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
    public class CreateRoleCommandHandlerTests : CommandHandlerTestBase
    {
        [Fact]
        public async void Handle_Should_Create_And_Return_Role()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var command = new CreateRoleCommand("test", organization.Id);
            var handler = new CreateRoleCommandHandler(OrganizationRepositoryMock.Object);

            OrganizationRepositoryMock.Setup(x => x.GetAsync(organization.Id)).Returns(Task.FromResult(organization));

            // Act
            await handler.Handle(command, new CancellationToken());

            // Assert
            organization.Roles.Single().Name.Should().Be(command.RoleName);
            OrganizationRepositoryMock.Verify(x => x.Update(organization), Times.Once());
        }
    }
}