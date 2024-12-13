using DockerHubBackend.Dto.Request;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace DockerHubBackend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {        
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto credentials)
        {
            var response = await _authService.Login(credentials);
            var tokenExpiration = Convert.ToInt32(_configuration["JWT:Expiration"]);
            Response.Headers.AccessControlAllowCredentials = "true";
            Response.Headers.AccessControlAllowHeaders = "true";
            Response.Cookies.Append(_configuration["JWT:CookieName"], response.Jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiration)
            });
            return Ok(response.Response);
        }
    }
}
