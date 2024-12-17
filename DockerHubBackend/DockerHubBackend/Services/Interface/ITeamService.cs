using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface ITeamService
    {
        public Task<ICollection<TeamDto>?> GetTeams(Guid organizationId);

        public Task<TeamDto> Create(TeamDto teamDto);

        public Task<TeamDto> AddMembers(Guid teamId, ICollection<MemberDto> memberDtos);

        public Task<TeamPermissionResponseDto> AddPermissions(TeamPermissionRequestDto teamPermissionDto);

        public Task<Team> Update(Team team);
        
    }
}
