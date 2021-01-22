using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.DomainEvents
{
    public class OrganizationCreatedDomainEvent : CreatedDomainEvent
    {
        public string OrganizationName { get; private set; }

        public OrganizationCreatedDomainEvent(Organization organization) : base(organization)
        {
            OrganizationName = organization.Name;
        }
    }
}