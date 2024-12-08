using DockerHubBackend.Models;
using DockerHubBackend.Repository.Utils;

namespace DockerHubBackend.Repository.Interface
{
    public interface IUserRepository : ICrudRepository<BaseUser>
    {
        Task<BaseUser?> GetUserByEmail(string email);
    }
}
