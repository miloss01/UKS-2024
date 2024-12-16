using System.Diagnostics.CodeAnalysis;
using System.Security.Policy;
using DockerHubBackend.Models;

namespace DockerHubBackend.Dto.Response
{
    public class TeamResponseDto
    {
        public required string Name { get; set; }
        public ICollection<MemberDto> Members { get; set; } = new HashSet<MemberDto>();
        public string? Description { get; set; }

        [SetsRequiredMembers]
        public TeamResponseDto(string name, string desription, ICollection<MemberDto> members) 
        { 
            Name = name;
            Members = members;
            Description = desription;
        }

        [SetsRequiredMembers]
        public TeamResponseDto(Team team)
        {
            ICollection<MemberDto> memberDtos = new HashSet<MemberDto>();
            foreach (StandardUser user in team.Members) memberDtos.Add(new MemberDto { Email = user.Email});
            Name = team.Name;
            Members = memberDtos;
            Description = team.Description;
        }

    }
}
