using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;
using OrganizationService.Infrastructure.OrganizationRepository;

namespace OrganizationService.Application.Commands
{
    /// <summary>
    /// Command handler for creating a department.
    /// </summary>
    public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, Department>
    {
        private readonly IOrganizationRepository _repository;

        public CreateDepartmentCommandHandler(IOrganizationRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new department for the organization specified by the command.
        /// </summary>
        /// <returns>Task object with the created department as result.</returns>
        public async Task<Department> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var organization = await _repository.GetAsync(command.OrganizationId);
            var department = organization.CreateDepartment(command.Name);
            
            _repository.Update(organization);

            return department;
        }
    }
}