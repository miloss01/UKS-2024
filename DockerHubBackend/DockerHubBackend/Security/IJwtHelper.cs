
namespace DockerHubBackend.Security
{
    public interface IJwtHelper
    {
        string GenerateToken(string role, string userId, DateTime? lastPasswordChange);
    }
}