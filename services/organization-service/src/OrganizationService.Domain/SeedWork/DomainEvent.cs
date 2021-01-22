using System;

namespace OrganizationService.Domain.SeedWork
{
    /// <summary>
    /// Represents an event that occurs in the domain.
    /// </summary>
    public abstract class DomainEvent
    {
        public DomainObject DomainObject { get; private set; }
        public DateTimeOffset Timestamp { get; private set; }

        protected DomainEvent(DomainObject domainObject)
        {
            DomainObject = domainObject;
            Timestamp = DateTimeOffset.UtcNow;
        }
    }
}