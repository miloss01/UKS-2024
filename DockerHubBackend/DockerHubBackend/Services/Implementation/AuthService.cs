using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;        
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IJwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, IJwtHelper jwtHelper, IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponse> Login(LoginCredentialsDto credentials)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);
            if (user == null)
            {
                throw new UnauthorizedException("Wrong email or password");
            }
            if(!user.IsVerified)
            {
                throw new UnauthorizedException("Account not verified");
            }
            if (_passwordHasher.VerifyHashedPassword(String.Empty, user.Password, credentials.Password) != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedException("Wrong email or password");
            }

            var token = _jwtHelper.GenerateToken(user.GetType().Name, user.Id.ToString(), user.Email, user.IsVerified);

            LoginResponse response = new LoginResponse {
                AccessToken = token
            };
            return response;
        }
    }
}
