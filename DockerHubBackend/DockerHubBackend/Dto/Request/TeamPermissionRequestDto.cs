using Microsoft.Extensions.Primitives;

namespace DockerHubBackend.Dto.Request
{
    public class TeamPermissionRequestDto
    {
        public required string TeamId { get; set; }
        public required string RepositoryId { get; set; }
        public required string Permission {  get; set; }
    }
}
