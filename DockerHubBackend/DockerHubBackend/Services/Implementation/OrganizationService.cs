using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
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

        public async Task<Organization?> AddOrganization(AddOrganizationDto organization)
        {
            return await _orgRepository.AddOrganization(organization);
        }

        public async Task<List<OrganizationOwnershipDto>?> GetOrganizations(string email)
        {
            return await _orgRepository.GetUserOrganizations(email);
        }
    }
}
