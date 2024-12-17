using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IOrganizationService
    {
        Task<Organization?> AddOrganization(AddOrganizationDto organization);
        Task<List<Organization>?> GetOrganizations(string email);
    }
}
