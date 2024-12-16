using System.Collections.ObjectModel;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;
using Microsoft.VisualBasic;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Repository.Interface
{
    public interface ITeamRepository : ICrudRepository<Team>
    {

        Task<ICollection<TeamDto>?> GetTeamsByOrganizationId(Guid organizationId);

    }
}
