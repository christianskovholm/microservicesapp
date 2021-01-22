using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Application.Commands
{
    [DataContract]
    public class CreateRoleCommand : IRequest<Role>
    {
        [Required]
        [DataMember]
        [StringLength(100)]
        public string RoleName { get; set; }
        public int OrganizationId { get; set; }
    }
}