using System.Security.Policy;
using DockerHubBackend.Models;

namespace DockerHubBackend.Dto.Response
{
    public class TeamResponseDto
    {
        public required string Name { get; set; }
        public ICollection<MemberDto> Members { get; set; } = new HashSet<MemberDto>();
        public string? Description { get; set; }
    }
}
