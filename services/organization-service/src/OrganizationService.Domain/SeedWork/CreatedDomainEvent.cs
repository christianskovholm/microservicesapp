namespace OrganizationService.Domain.SeedWork
{
    /// <summary>
    /// Represents an event that creates a domain object.
    /// </summary>
    public abstract class CreatedDomainEvent : DomainEvent
    {
        protected CreatedDomainEvent(DomainObject domainObject) : base(domainObject)
        {
        }
    }
}