using DockerHubBackend.Dto.Request;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
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
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public OrganizationController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Product data is null");
            }

            //// Dodavanje proizvoda u bazu
            //_context.Products.Add(product);
            //await _context.SaveChangesAsync();

            //// Vracanje odgovora sa status kodom 201 (Created)
            //return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            return Ok();
        }
    }
}
