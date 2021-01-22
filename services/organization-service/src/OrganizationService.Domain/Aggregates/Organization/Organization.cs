using System.Collections.Generic;
using Domain.Exceptions;
using OrganizationService.Domain.DomainEvents;
using OrganizationService.Domain.SeedWork;

namespace OrganizationService.Domain.Aggregates.Organization
{
    /// <summary>
    /// Represents an organization and is the aggregate root.
    /// Contains methods for manipulating the parts of the organization.
    /// </summary>
    public class Organization : AggregateRoot
    {
        private List<Department> _departments;
        public IReadOnlyCollection<Department> Departments => _departments.AsReadOnly();
        public string Description { get; private set; }
        public string Name { get; private set; }
        private List<Role> _roles;
        public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

        public Organization(string description, string name)
        {
            _departments = new List<Department>();
            Description = description;
            Name = name;
            _roles = new List<Role>();
        }

        /// <summary>
        /// Creates a new department in the organization.
        /// </summary>
        /// <returns>The created Department.</returns>
        public Department CreateDepartment(string name)
        {
            var exists = _departments.Exists(x => x.Name == name);

            if (exists) 
                throw new DomainException($"A department named {name} already exists.");

            var department = new Department(name);
            
            _departments.Add(department);
            AddDomainEvent(new DepartmentCreatedDomainEvent(department));

            return department;
        }

        /// <summary>
        /// Creates a new role in the organization.
        /// </summary>
        /// <returns>The created Role.</returns>
        public Role CreateRole(string name)
        {
            var exists = _roles.Exists(x => x.Name == name);

            if (exists) 
                throw new DomainException($"A role named {name} already exists.");

            var role = new Role(name);

            _roles.Add(role);
            AddDomainEvent(new RoleCreatedDomainEvent(Id, role));
        
            return role;
        }
    }
}