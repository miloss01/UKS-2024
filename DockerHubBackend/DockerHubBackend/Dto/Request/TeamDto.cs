using System.Diagnostics.CodeAnalysis;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Dto.Request
{
    public class TeamDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public ICollection<MemberDto> Members { get; set; }
        public Guid OrganizationId { get; set; }

        [SetsRequiredMembers]
        public TeamDto()
        {

        }

        [SetsRequiredMembers]
        public TeamDto(string name, string desription, ICollection<MemberDto> members, Guid organizationId)
        {
            Name = name;
            Members = members;
            Description = desription;
            OrganizationId = organizationId;
        }

        [SetsRequiredMembers]
        public TeamDto(string name, string desription, ICollection<MemberDto> members)
        {
            Name = name;
            Members = members;
            Description = desription;
        }

        [SetsRequiredMembers]
        public TeamDto(Team team)
        {
            ICollection<MemberDto> memberDtos = new HashSet<MemberDto>();
            foreach (StandardUser user in team.Members) memberDtos.Add(new MemberDto { Email = user.Email });
            Name = team.Name;
            Members = memberDtos;
            Description = team.Description;
        }
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
