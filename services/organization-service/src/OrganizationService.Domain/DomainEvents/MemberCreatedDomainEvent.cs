using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.DomainEvents
{
    public class MemberCreatedDomainEvent : CreatedDomainEvent
    {
        public int DepartmentId { get; private set; }
        public string MemberName { get; private set; }

        public MemberCreatedDomainEvent(int departmentId, Member member) : base(member)
        {
            DepartmentId = departmentId;
            MemberName = member.Name;
        }
    }
}