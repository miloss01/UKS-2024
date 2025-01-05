using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response.Organization;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Implementation;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace DockerHubBackend.Services.Implementation
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _orgRepository;
        private readonly ILogger<OrganizationService> _logger;

        public OrganizationService(IOrganizationRepository organizationRepository, ILogger<OrganizationService> logger)
        {
            _orgRepository = organizationRepository;
            _logger = logger;
        }

        public async Task<Guid?> AddOrganization(AddOrganizationDto organization)
        {
            _logger.LogInformation("Adding a new organization: {OrganizationName}", organization.Name);
            var result = await _orgRepository.AddOrganization(organization);
            if (result == null)
            {
                _logger.LogWarning("Failed to add organization: {OrganizationName}", organization.Name);
            }
            else
            {
                _logger.LogInformation("Organization added successfully with ID: {OrganizationId}", result);
            }
            return result;
        }

        public async Task<List<OrganizationOwnershipDto>?> GetOrganizations(string email)
        {
            _logger.LogInformation("Fetching organizations for user: {Email}", email);
            var organizations = await _orgRepository.GetUserOrganizations(email);
            if (organizations == null)
            {
                _logger.LogWarning("No organizations found for user: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Found {Count} organizations for user: {Email}", organizations.Count, email);
            }
            return organizations;
        }

        public async Task<Organization?> GetOrganizationById(Guid id)
        {
            _logger.LogInformation("Fetching organization with ID: {OrganizationId}", id);
            var organization = await _orgRepository.GetOrganizationById(id);
            if (organization == null)
            {
                _logger.LogWarning("Organization with ID: {OrganizationId} not found", id);
            }
            else
            {
                _logger.LogInformation("Organization with ID: {OrganizationId} fetched successfully", id);
            }
            return organization;
        }

        public async Task<OrganizationUsersDto> GetListUsersByOrganizationId(Guid organizationId)
        {
            _logger.LogInformation("Fetching users for organization with ID: {OrganizationId}", organizationId);
            var users = await _orgRepository.GetListUsersByOrganizationId(organizationId);
            if (users == null)
            {
                _logger.LogWarning("No users found for organization with ID: {OrganizationId}", organizationId);
            }
            else
            {
                _logger.LogInformation("Found users for organization with ID: {OrganizationId}", organizationId);
            }
            return users;
        }

        public async Task<string> AddMemberToOrganization(Guid organizationId, Guid userId)
        {
            _logger.LogInformation("Adding user with ID: {UserId} to organization with ID: {OrganizationId}", userId, organizationId);
            var result = await _orgRepository.AddMemberToOrganization(organizationId, userId);
            if (result == "User added to organization successfully.")
            {
                _logger.LogInformation("User with ID: {UserId} added to organization with ID: {OrganizationId} successfully", userId, organizationId);
            }
            else
            {
                _logger.LogWarning("Failed to add user with ID: {UserId} to organization with ID: {OrganizationId}. Reason: {Reason}", userId, organizationId, result);
            }
            return result;
        }

        public async Task DeleteOrganization(Guid organizationId)
        {
            _logger.LogInformation("Deleting organization with ID: {OrganizationId}", organizationId);
            await _orgRepository.DeleteOrganization(organizationId);
            _logger.LogInformation("Organization with ID: {OrganizationId} deleted successfully", organizationId);
        }

        public async Task UpdateOrganization(Guid organizationId, string imageLocation, string description)
        {
            _logger.LogInformation("Updating organization with ID: {OrganizationId}", organizationId);
            await _orgRepository.UpdateOrganization(organizationId, imageLocation, description);
            _logger.LogInformation("Organization with ID: {OrganizationId} updated successfully", organizationId);
        }
    }
}
