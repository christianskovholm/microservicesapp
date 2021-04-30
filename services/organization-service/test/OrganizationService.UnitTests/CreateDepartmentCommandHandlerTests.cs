using System;
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
    public class CreateDepartmentCommandHandlerTests : CommandHandlerTestBase
    {
        [Fact]
        public async void Handle_Should_Create_And_Return_Department()
        {
            // Arrange
            var datetime = DateTimeOffset.UtcNow;
            var organization = new Organization("test", "test");
            var command = new CreateDepartmentCommand("test", organization.Id);
            var handler = new CreateDepartmentCommandHandler(OrganizationRepositoryMock.Object);

            OrganizationRepositoryMock.Setup(x => x.GetAsync(organization.Id)).Returns(Task.FromResult(organization));

            // Act
            await handler.Handle(command, new CancellationToken());

            // Assert
            organization.Departments.Single().Name.Should().Be(command.Name);
            OrganizationRepositoryMock.Verify(x => x.Update(organization), Times.Once());
        }
    }
}