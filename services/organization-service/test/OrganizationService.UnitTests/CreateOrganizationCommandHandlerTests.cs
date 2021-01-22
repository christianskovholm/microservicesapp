using System.Threading;
using FluentAssertions;
using Moq;
using OrganizationService.Application.Commands;
using Xunit;

namespace OrganizationService.UnitTests
{
    public class CreateOrganizationCommandHandlerTests : CommandHandlerTestBase
    {
        [Fact]
        public async void Handle_Should_Create_Organization()
        {
            // Arrange
            var command = new CreateOrganizationCommand { Name = "test", Description = "test" };
            var handler = new CreateOrganizationCommandHandler(OrganizationRepositoryMock.Object);

            // Act
            var organization = await handler.Handle(command, new CancellationToken());

            // Assert
            organization.Name.Should().Be(command.Name);
            organization.Description.Should().Be(command.Description);
            OrganizationRepositoryMock.Verify(x => x.Add(organization), Times.Once());
        }
    }
}