using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.DomainEvents
{
    public class RoleCreatedDomainEvent : CreatedDomainEvent
    {
        public int DepartmentId { get; private set; }
        public string RoleName { get; private set; }

        public RoleCreatedDomainEvent(int departmentId, Role role) : base(role)
        {
            DepartmentId = departmentId;
            RoleName = role.Name;
        }
    }
}