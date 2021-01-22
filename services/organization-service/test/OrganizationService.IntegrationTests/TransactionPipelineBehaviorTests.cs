using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using MediatR;
using Moq;
using OrganizationService.Application.Behaviors;
using OrganizationService.Application.Commands;
using OrganizationService.Application.Factories;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure;
using Xunit;

namespace OrganizationService.IntegrationTests
{
    public class TransactionPipelineBehaviorTests : IDisposable
    {
        private readonly OrganizationDbContext _context;

        public TransactionPipelineBehaviorTests()
        {
            _context = new OrganizationDbContextFactory().CreateDbContext(null);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async void Handle_Should_Create_Event_For_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Organization>>();
            var handler = new TransactionPipelineBehavior<CreateOrganizationCommand, Organization>(_context);

            _context.Add(organization);

            // Act
            await handler.Handle(new Mock<CreateOrganizationCommand>().Object, new CancellationToken(), requestHandlerDelegateMock.Object);

            // Assert
            var e = _context.Events.Single(x => x.OrganizationId == organization.Id);

            e.Timestamp.Should().Be(organization.LastUpdated.UtcDateTime);
            e.Timestamp.Should().Be(organization.Created.UtcDateTime);
            e.EventType.Should().Be("organization_created");
        }

        [Fact]
        public async void Handle_Should_Create_Event_For_Organization_Child_DomainObject()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Department>>();
            var handler = new TransactionPipelineBehavior<CreateDepartmentCommand, Department>(_context);

            organization.CreateDepartment("test");
            _context.Organizations.Add(organization);

            // Act
            await handler.Handle(new Mock<CreateDepartmentCommand>().Object, new CancellationToken(), requestHandlerDelegateMock.Object);            

            // Assert
            var e = _context.Events.Single(x => x.OrganizationId == organization.Id && x.EventType == "department_created");

            organization.Departments.Single().LastUpdated.Should().Be(e.Timestamp);
            organization.Departments.Single().Created.Should().Be(e.Timestamp);
        }
    }
}