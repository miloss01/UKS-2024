using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;

namespace DockerHubBackend.Services.Interface
{
    public interface IUserService
    {
        Task ChangePassword(ChangePasswordDto changePasswordDto);
        Task<StandardUserDto> RegisterStandardUser(RegisterUserDto registerUserDto);
    }
}
