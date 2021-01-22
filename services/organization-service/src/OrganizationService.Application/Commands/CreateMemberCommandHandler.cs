using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Exceptions;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.Application.Commands
{
    /// <summary>
    /// Command handler for creating a member.
    /// </summary>
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Member>
    {
        private readonly IOrganizationRepository _repository;

        public CreateMemberCommandHandler(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new member for the department specified by the command. 
        /// Throws a DomainObjectNotFoundException if department doesn't exist.
        /// </summary>
        /// <returns>Task object with the created member as result.</returns>        
        public async Task<Member> Handle(CreateMemberCommand command, CancellationToken cancellationToken)
        {
            var organization = await _repository.GetAsync(command.OrganizationId);
            var role = organization.Roles.SingleOrDefault(x => x.Id == command.RoleId);

            if (role == null) 
                throw new DomainException($"{organization.Name} has no role with id {command.RoleId}.");

            var department = organization.Departments.SingleOrDefault(x => x.Id == command.DepartmentId);

            if (department == null)
                throw new DomainObjectNotFoundException($"{organization.Name} has no department with id: {command.DepartmentId}.");

            var member = department.CreateMember(command.Name, role);
            _repository.Update(organization);

            return member;
        }
    }
}