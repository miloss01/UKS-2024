using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IOrganizationRepository : ICrudRepository<BaseUser>
    {
        Task<Guid?> AddOrganization(AddOrganizationDto organization);
        Task<List<OrganizationOwnershipDto>?> GetUserOrganizations(string email);
    }
}
