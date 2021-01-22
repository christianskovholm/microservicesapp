using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.Aggregates.Organization
{
    /// <summary>
    /// Represents a person in a specific department
    /// of an organization.
    /// </summary>
    public class Member : DomainObject
    {
        public string Name { get; private set; }
        public Role Role { get; private set; }

        public Member() {}

        public Member(string name, Role role)
        {
            Name = name;
            Role = role;
        }
    }
}