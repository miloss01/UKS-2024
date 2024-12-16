﻿using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok();
        }

    }
}
