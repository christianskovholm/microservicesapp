using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using OrganizationService.Application.Commands;
using OrganizationService.Application.Controllers;
using OrganizationService.Application.Responses;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure.Queries;
using Xunit;

namespace OrganizationService.UnitTests
{
    public class OrganizationsControllerUnitTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly OrganizationsController _controller;
        private readonly Mock<IQueries> _mockQueries;

        public OrganizationsControllerUnitTests()
        {
            _mockMediator = new Mock<IMediator>();
            _mockQueries = new Mock<IQueries>();
            _controller = new OrganizationsController(_mockMediator.Object, _mockQueries.Object);
        }

        [Fact]
        public async void Get_Should_Return_Organization()
        {
            // Arrange
            var organizationId = 1;
            var organization = new JObject();
            var expectedResult = new HttpResponseBody(organization);
            
            _mockQueries.Setup(x => x.GetOrganizationAsync(organizationId)).ReturnsAsync(organization);

            // Act
            var result = await _controller.Get(organizationId) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void Post_Should_Return_Id_Of_Created_Organization()
        {
            // Arrange
            var organization = new Organization("test", "test");
            var expectedResult = new HttpResponseBody(new IdResponse(organization.Id));
            var command = new CreateOrganizationCommand { Name = "test", Description = "test" };

            _mockMediator
                .Setup(x => x.Send(It.Is<CreateOrganizationCommand>(x => x == command), new CancellationToken()))
                .ReturnsAsync(organization);

            // Act
            var result = await _controller.Post(command) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void CreateDepartment_Should_Return_Id_Of_Created_Department()
        {
            // Arrange
            var organizationId = 1;
            var department = new Department("test");
            var command = new CreateDepartmentCommand { Name = "test" };
            var expectedResult = new HttpResponseBody(new IdResponse(department.Id));

            _mockMediator
                .Setup(x => x.Send(It.Is<CreateDepartmentCommand>(x => x.Name == command.Name && x.OrganizationId == organizationId), new CancellationToken()))
                .ReturnsAsync(department);

            // Act
            var result = await _controller.CreateDepartment(command, organizationId) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void CreateRole_Should_Return_Id_Of_Created_Role()
        {
            // Arrange
            var organizationId = 1;
            var role = new Role("test");
            var command = new CreateRoleCommand { RoleName = "test" };
            var expectedResult = new HttpResponseBody(new IdResponse(role.Id));

            _mockMediator.
                Setup(x => x.Send(It.Is<CreateRoleCommand>(x => x.RoleName == command.RoleName && 
                                                                x.OrganizationId == organizationId),
                                                                new CancellationToken()))
                .ReturnsAsync(role);

            // Act
            var result = await _controller.CreateRole(command, organizationId) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void CreateMember_Should_Return_Id_Of_Created_Member()
        {
            // Arrange
            var departmentId = 1;
            var organizationId = 1;
            var member = new Member("test", new Role("test"));
            var command = new CreateMemberCommand { Name = "test", RoleId = 1 };
            var expectedResult = new HttpResponseBody(new IdResponse(member.Id));

            _mockMediator.
                Setup(x => x.Send(It.Is<CreateMemberCommand>(x => x.Name == command.Name && 
                                                                  x.OrganizationId == organizationId &&
                                                                  x.DepartmentId == departmentId &&
                                                                  x.RoleId == command.RoleId),
                                                                  new CancellationToken()))
                .ReturnsAsync(member);

            // Act
            var result = await _controller.CreateMember(command, organizationId, departmentId) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}