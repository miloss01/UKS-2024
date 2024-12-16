using System.Collections.ObjectModel;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;
using Microsoft.VisualBasic;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Dto.Request;

namespace DockerHubBackend.Repository.Interface
{
    public interface ITeamRepository : ICrudRepository<Team>
    {

        Task<ICollection<TeamResponseDto>> GetByOrganizationId(Guid organizationId);

        Task<Team> GetByName(string name);

        Task<ICollection<StandardUser>> GetMembers(Guid id);
    }
}
