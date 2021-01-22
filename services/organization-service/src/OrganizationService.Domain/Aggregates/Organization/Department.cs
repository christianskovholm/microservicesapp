using System.Collections.Generic;
using OrganizationService.Domain.DomainEvents;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.Aggregates.Organization
{
    /// <summary>
    /// Represents a department within the organization. Contains members and roles of the organization,
    /// and methods for manipulating these.
    /// </summary>
    public class Department : DomainObject
    {
        private List<Member> _members;
        public IReadOnlyCollection<Member> Members => _members.AsReadOnly();
        public string Name { get; private set; }

        public Department(string name)
        {
            Name = name;
            _members = new List<Member>();
        }

        /// <summary>
        /// Creates a member with the specified name and role.
        /// </summary>
        /// <returns>Member representing the created member.</returns>
        public Member CreateMember(string name, Role role)
        {
            var member = new Member(name, role);

            _members.Add(member);
            AddDomainEvent(new MemberCreatedDomainEvent(Id, member));

            return member;
        }
    }
}