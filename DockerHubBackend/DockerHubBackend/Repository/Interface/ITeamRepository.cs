using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface ITeamRepository : ICrudRepository<Team>
    {

        Task<List<Team>?> GetTeamsByOrganizationId(Guid organizationId);

    }
}
