using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IOrganizationService
    {
        Task<Organization> AddOrganization(Organization organization);
    }
}
