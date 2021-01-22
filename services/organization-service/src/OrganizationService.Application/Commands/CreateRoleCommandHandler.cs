using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.Application.Commands
{
    /// <summary>
    /// Command handler for creating a role.
    /// </summary>
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Role>
    {
        private readonly IOrganizationRepository _repository;

        public CreateRoleCommandHandler(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new role for the department specified by the command. 
        /// Throws a DomainObjectNotFoundException in case department doesn't exist.
        /// </summary>
        /// <returns>Task object with the created role as result.</returns>     
        public async Task<Role> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var organization = await _repository.GetAsync(command.OrganizationId);
            var role = organization.CreateRole(command.RoleName);

            _repository.Update(organization);

            return role;
        }
    }
}