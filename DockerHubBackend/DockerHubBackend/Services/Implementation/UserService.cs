using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
