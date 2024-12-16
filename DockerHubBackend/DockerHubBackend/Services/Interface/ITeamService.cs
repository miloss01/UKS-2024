using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface ITeamService
    {
        public Task<ICollection<TeamDto>?> GetTeams(Guid organizationId);

        public Team Create(Team team);

        public Team AddMembers(Guid teamId, ICollection<StandardUser> members);

        public Team ChangePersmissions(Guid teamId, PermissionType permissionType);

        public Team Update(Team team);
        
    }
}
