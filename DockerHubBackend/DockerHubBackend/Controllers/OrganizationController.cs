using DockerHubBackend.Dto.Request;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Implementation;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace DockerHubBackend.Controllers
{
    [Route("api/organization")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _orgService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _orgService = organizationService;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid dto");
            }
            var addedOrganization = await _orgService.AddOrganization(dto);
            if(addedOrganization == null)
            {
                return BadRequest("Error database saving");
            }

            return Ok(addedOrganization);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserOrganizations(string email)
        {
            try
            {
                var organizations = await _orgService.GetOrganizations(email);

                if (organizations == null || !organizations.Any())
                {
                    return NotFound("User is not a member or owner of any organization.");
                }

                return Ok(organizations);  
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetOrganizationById(Guid id)
        {
            var organization = await _orgService.GetOrganizationById(id);

            if (organization == null)
            {
                return NotFound(new { message = "Organization not found" });
            }

            return Ok(organization);
        }
    }
}
