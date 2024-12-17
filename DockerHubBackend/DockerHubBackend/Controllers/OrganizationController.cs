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
        // todo promeni ovo da uzima dto, a ne organizaciju iz body-a
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            Console.WriteLine("TU SMOOOOOOOOOOOOOOOOOOO");
            //if (dto == null)
            //{
            //    return BadRequest("Product data is null");
            //}
            //var addedOrganization = await _orgService.AddOrganization(dto);

            return Ok();
        }
    }
}
