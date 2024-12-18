using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Services.Interface
{
    public interface IUserService
    {
        Task ChangePassword(ChangePasswordDto changePasswordDto);
        Task<CreatedUserDto> RegisterStandardUser(RegisterUserDto registerUserDto);
    }
}
