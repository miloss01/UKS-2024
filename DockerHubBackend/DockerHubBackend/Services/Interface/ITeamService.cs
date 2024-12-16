using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface ITeamService
    {
        public Task<ICollection<TeamResponseDto>?> GetTeams(Guid organizationId);

        public Task<TeamResponseDto> Create(TeamRequestDto teamDto);

        public Task<TeamResponseDto> AddMembers(Guid teamId, ICollection<MemberDto> memberDtos);

        public Task<Team> ChangePersmissions(Guid teamId, PermissionType permissionType);

        public Task<Team> Update(Team team);
        
    }
}
