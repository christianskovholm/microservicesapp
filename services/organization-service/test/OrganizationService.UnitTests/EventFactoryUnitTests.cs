using FluentAssertions;
using Newtonsoft.Json.Linq;
using OrganizationService.Application.Factories;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.DomainEvents;
using Xunit;

namespace OrganizationService.UnitTests
{
    public class EventFactoryUnitTests
    {

        [Fact]
        public void Create_Should_Create_Event()
        {
            // Arrange
            var organizationId = 1;
            var department = new Department("test");
            var domainEvent = new DepartmentCreatedDomainEvent(department);

            // Act
            var e = EventFactory.Create(domainEvent, organizationId);

            // Assert
            var payload = JObject.Parse(e.Payload);

            payload.Value<string>("departmentName").Should().Be(domainEvent.DepartmentName);
            e.EventType.Should().Be("department_created");
            e.OrganizationId.Should().Be(organizationId);
        }
    }
}