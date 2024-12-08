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
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper, IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponseWithJwt> Login(LoginCredentialsDto credentials)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);
            if (user == null)
            {
                throw new BadRequestException("Wrong email or password");
            }            
            if (_passwordHasher.VerifyHashedPassword(String.Empty, user.Password, credentials.Password) != PasswordVerificationResult.Success)
            {
                throw new BadRequestException("Wrong email or password");
            }

            var token = _jwtHelper.GenerateToken(user.GetType().Name, user.Id.ToString(), user.LastPasswordChangeDate);

            LoginResponse loginResponse = new LoginResponse {
                UserEmail = user.Email,
                UserId = user.Id.ToString(),
                UserRole = user.GetType().Name
            };
            LoginResponseWithJwt response = new LoginResponseWithJwt
            {
                Response = loginResponse,
                Jwt = token
            };
            return response;
        }
    }
}
