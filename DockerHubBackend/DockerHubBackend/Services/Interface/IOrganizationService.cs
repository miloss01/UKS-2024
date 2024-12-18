using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response.Organization;
using DockerHubBackend.Models;

namespace DockerHubBackend.Services.Interface
{
    public interface IOrganizationService
    {
        Task<Guid?> AddOrganization(AddOrganizationDto organization);
        Task<List<OrganizationOwnershipDto>?> GetOrganizations(string email);
        Task<Organization?> GetOrganizationById(Guid id);
        Task<OrganizationUsersDto> GetListUsersByOrganizationId(Guid organizationId);
    }
}
