using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IOrganizationService
    {
        Task<Organization?> AddOrganization(AddOrganizationDto organization);
        Task<List<OrganizationOwnershipDto>?> GetOrganizations(string email);
    }
}
