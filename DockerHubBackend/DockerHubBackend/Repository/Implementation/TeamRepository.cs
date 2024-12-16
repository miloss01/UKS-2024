using System.Runtime.CompilerServices;
using DockerHubBackend.Data;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Implementation
{
    public class TeamRepository : CrudRepository<Team>, ITeamRepository
    {

        public TeamRepository(DataContext context) : base(context) { }

        public async Task<List<Team>> GetTeamsByOrganizationId(Guid organizationId)
        {
            throw new NotImplementedException();
        }
    }
}
