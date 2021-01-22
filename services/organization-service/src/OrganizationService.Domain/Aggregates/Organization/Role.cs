using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.Aggregates.Organization
{
    /// <summary>
    /// Represents a role in a specific department of an organization.
    /// </summary>
    public class Role : DomainObject
    {
        public string Name { get; private set; }

        public Role() {}

        public Role(string name)
        {
            Name = name;
        }
    }
}