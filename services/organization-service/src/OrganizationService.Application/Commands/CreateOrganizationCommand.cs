using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Application.Commands
{
    [DataContract]
    public class CreateOrganizationCommand : IRequest<Organization>
    {
        [Required]
        [DataMember]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [DataMember]
        [StringLength(50)]
        public string Name { get; set; }
    }
}