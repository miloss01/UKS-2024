using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface ITeamService
    {
        public Task<ICollection<TeamResponseDto>?> GetTeams(Guid organizationId);

        public Task<TeamResponseDto> Create(TeamRequestDto teamDto);

        public Team AddMembers(Guid teamId, ICollection<StandardUser> members);

        public Team ChangePersmissions(Guid teamId, PermissionType permissionType);

        public Team Update(Team team);
        
    }
}
