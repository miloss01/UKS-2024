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

        public async Task<Guid?> AddOrganization(AddOrganizationDto organization)
        {
            return await _orgRepository.AddOrganization(organization);
        }

        public async Task<List<OrganizationOwnershipDto>?> GetOrganizations(string email)
        {
            return await _orgRepository.GetUserOrganizations(email);
        }

        public async Task<Organization?> GetOrganizationById(Guid id)
        {
            return await _orgRepository.GetOrganizationById(id);
        }

        public async Task<List<MemberDto>> GetMembersByOrganizationIdAsync(Guid organizationId)
        {
            return await _orgRepository.GetMembersByOrganizationIdAsync(organizationId);
        }
    }
}
