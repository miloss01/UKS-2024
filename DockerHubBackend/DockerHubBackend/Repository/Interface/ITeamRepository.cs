using System.Collections.ObjectModel;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;
using Microsoft.VisualBasic;

namespace DockerHubBackend.Repository.Interface
{
    public interface ITeamRepository : ICrudRepository<Team>
    {

        Task<ICollection<Team>?> GetTeamsByOrganizationId(Guid organizationId);

    }
}
