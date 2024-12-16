using System.Collections.ObjectModel;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DockerHubBackend.Models;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Dto.Request;

namespace DockerHubBackend.Controllers
{
    [Route("api/team")]
    [ApiController]
    public class TeamController : ControllerBase
    {

        private readonly ITeamService _teamService;
        private readonly IConfiguration _configuration;

        public TeamController(ITeamService teamService, IConfiguration configuration)
        {
            _teamService = teamService;
            _configuration = configuration;
        }

        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetTeamsByOrganizationId([FromRoute] Guid organizationId)
        {
            ICollection<TeamResponseDto> teams = await _teamService.GetTeams(organizationId);
            return Ok(teams);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamRequestDto teamDto)
        {
            TeamResponseDto team = await _teamService.Create(teamDto);
            return Ok(team);
        }

    }
}
