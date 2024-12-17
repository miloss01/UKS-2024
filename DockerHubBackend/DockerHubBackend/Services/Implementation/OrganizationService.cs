using DockerHubBackend.Models;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _orgRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _orgRepository = organizationRepository;
        }

        public async Task<Organization> AddOrganization(Organization organization)
        {
            // add logic (validacija, provera, itd.)
            return await _orgRepository.AddOrganization(organization);
        }
    }
}
