using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.Application.Commands
{
    /// <summary>
    /// Command handler for creating an organization.
    /// </summary>
    public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Organization>
    {
        private readonly IOrganizationRepository _repository;

        public CreateOrganizationCommandHandler(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new organization with the information specified by the command.
        /// </summary>
        /// <returns>Task object with the created organization as result.</returns>      
        public Task<Organization> Handle(CreateOrganizationCommand command, CancellationToken cancellationToken)
        {
            var organization = new Organization(command.Description, command.Name);
            
            _repository.Add(organization);

            return Task.FromResult(organization);
        }
    }
}