using DockerHubBackend.Dto.Request;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DockerHubBackend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPatch("password/change")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            await _userService.ChangePassword(changePasswordDto);
            return NoContent();
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto changePasswordDto)
        {
            var response = await _userService.RegisterStandardUser(changePasswordDto);
            return Ok(response);
        }
    }

}
