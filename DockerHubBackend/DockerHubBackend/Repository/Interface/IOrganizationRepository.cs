using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IOrganizationRepository : ICrudRepository<BaseUser>
    {
        Task<Organization?> AddOrganization(AddOrganizationDto organization);
        Task<List<Organization>?> GetUserOrganizations(string email);
    }
}
