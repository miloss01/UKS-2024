using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class TeamService : ITeamService
    {   
        private readonly ITeamRepository _repository;
        public TeamService(ITeamRepository repository) 
        {
            _repository = repository;
        }

        public ICollection<Team> GetTeams(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Team Create(Team team)
        {
            throw new NotImplementedException();
        }

        public Team AddMembers(Guid teamId, ICollection<StandardUser> members)
        {
            throw new NotImplementedException();
        }

        public Team ChangePersmissions(Guid teamId, PermissionType permissionType)
        {
            throw new NotImplementedException();
        }

        public Team Update(Team team)
        {
            throw new NotImplementedException();
        }
    }
}
