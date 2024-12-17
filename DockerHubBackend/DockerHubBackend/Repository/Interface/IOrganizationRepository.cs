using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IOrganizationRepository : ICrudRepository<BaseUser>
    {
        Task<BaseUser?> AddOrganization(string email);
    }
}
