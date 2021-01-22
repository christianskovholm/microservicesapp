using System;
using System.Collections.Generic;

namespace OrganizationService.Domain.SeedWork
{
    /// <summary>
    /// Represents an object in the domain and contains base properties
    /// common for all objects in the domain space.
    /// </summary>
    public abstract class DomainObject
    {   
        public DateTimeOffset Created { get; set; }
        private List<DomainEvent> _domainEvents;
        public int Id { get; private set; }
        public DateTimeOffset LastUpdated { get; set; }

        protected DomainObject()
        {
            _domainEvents = new List<DomainEvent>();
        }

        protected void AddDomainEvent(DomainEvent e)
        {
            _domainEvents.Add(e);
        }

        public IReadOnlyCollection<DomainEvent> GetDomainEvents()
        {
            return _domainEvents.AsReadOnly();
        }
    }
}