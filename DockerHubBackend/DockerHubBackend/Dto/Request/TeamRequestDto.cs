using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Dto.Request
{
    public class TeamRequestDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<MemberDto> Members { get; set; }
        public required Guid OrganizationId { get; set; }


        public Team ToTeam(Organization organization)
        {
            return new Team
            {
                Name = Name,
                Description = Description,
                OrganizationId = OrganizationId,
                Organization = organization
            };
        }
    }
}
