using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.DomainEvents
{
    public class DepartmentCreatedDomainEvent : CreatedDomainEvent
    {
        public string DepartmentName { get; private set; }

        public DepartmentCreatedDomainEvent(Department department) : base(department)
        {
            DepartmentName = department.Name;
        }
    }
}