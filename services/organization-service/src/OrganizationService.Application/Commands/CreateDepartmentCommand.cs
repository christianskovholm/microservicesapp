using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Application.Commands
{
    [DataContract]
    public class CreateDepartmentCommand : IRequest<Department>
    {
        [Required]
        [DataMember]
        [StringLength(50)]
        public string Name { get; set; }
        public int OrganizationId { get; set; }
    }
}