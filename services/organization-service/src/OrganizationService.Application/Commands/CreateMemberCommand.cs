using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using MediatR;
using OrganizationService.Domain.Aggregates.Organization;

namespace OrganizationService.Application.Commands
{
    [DataContract]
    public class CreateMemberCommand : IRequest<Member>
    {
        [Required]
        [DataMember]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [DataMember]
        [Range(1, int.MaxValue)]
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public int DepartmentId { get; set; }
    }
}