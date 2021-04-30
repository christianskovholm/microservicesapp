using System.ComponentModel.DataAnnotations;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Application.Commands
{
    public record Command<T> : IRequest<T> where T : DomainObject;
    public record CreateOrganizationCommand([Required][StringLength(200)] string Description, [Required][StringLength(50)] string Name) : Command<Organization>;
    public record CreateDepartmentCommand([Required][StringLength(50)] string Name, int OrganizationId) : Command<Department>;
    public record CreateRoleCommand([Required][StringLength(100)] string RoleName, int OrganizationId) : Command<Role>;
    public record CreateMemberCommand(
        [Required][StringLength(150)] string Name, 
        [Required][Range(1, int.MaxValue)] int RoleId,
        int OrganizationId,
        int DepartmentId
        ) : Command<Member>;
}