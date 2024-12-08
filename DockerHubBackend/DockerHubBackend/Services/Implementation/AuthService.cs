using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;

namespace DockerHubBackend.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JwtHelper jwtHelper, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login(LoginCredentialsDto credentials, HttpResponse response)
        {
            var user = await _userRepository.GetUserByEmail(credentials.Email);
            if (user == null)
            {
                throw new BadRequestException("Wrong email or password");
            }

            var token = _jwtHelper.GenerateToken(user.GetType().Name, user.Id.ToString());
            var tokenExpiration = Convert.ToInt32(_configuration["JWT:Expiration"]);
            response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(tokenExpiration)
            });
            LoginResponse loginResponse = new LoginResponse {
                UserEmail = user.Email,
                UserId = user.Id.ToString(),
                UserRole = user.GetType().Name
            };
            return loginResponse;
        }
    }
}
