using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrganizationService.Application.Commands;
using OrganizationService.Application.Responses;
using OrganizationService.Infrastructure.Queries;

namespace OrganizationService.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueries _queries;

        public OrganizationsController(IMediator mediator, IQueries queries)
        {
            _mediator = mediator;
            _queries = queries;
        }

        public override OkObjectResult Ok(object value)
        {
            var body = new HttpResponseBody(value);

            return base.Ok(body);
        }

        /// <summary>
        /// Returns the organization specified by the organization id.
        /// </summary>
        /// <returns>Task object with an ObjectResult as result.</returns>
        [HttpGet]
        [Route("{organizationId}")]
        public async Task<IActionResult> Get(int organizationId)
        {
            var response = await _queries.GetOrganizationAsync(organizationId);

            return Ok(response);
        }

        /// <summary>
        /// Creates the organization specified by the command.
        /// </summary>
        /// <returns>Task object with an ObjectResult as result.</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateOrganizationCommand command)
        {
            var organization = await _mediator.Send(command);
            var response = new IdResponse(organization.Id);

            return Ok(response);
        }

        /// <summary>
        /// Creates the department specified by the command.
        /// </summary>
        /// <returns>Task object with an ObjectResult as result.</returns>
        [HttpPost]
        [Route("departments")]
        public async Task<IActionResult> CreateDepartment([FromBody]CreateDepartmentCommand command)
        {
            var department = await _mediator.Send(command);
            var response = new IdResponse(department.Id);

            return Ok(response);
        }

        /// <summary>
        /// Creates the role specified by the command.
        /// </summary>
        /// <returns>Task object with an ObjectResult as result.</returns>
        [HttpPost]
        [Route("roles")]
        public async Task<IActionResult> CreateRole([FromBody]CreateRoleCommand command, int organizationId)
        {
            var role = await _mediator.Send(command);
            var response = new IdResponse(role.Id);

            return Ok(response);
        }

        /// <summary>
        /// Creates the member specified by the command.
        /// </summary>
        /// <returns>Task object with an ObjectResult as result.</returns>
        [HttpPost]
        [Route("departments/members")]
        public async Task<IActionResult> CreateMember([FromBody]CreateMemberCommand command)
        {
            var member = await _mediator.Send(command);
            var response = new IdResponse(member.Id);

            return Ok(response);
        }
    }
}