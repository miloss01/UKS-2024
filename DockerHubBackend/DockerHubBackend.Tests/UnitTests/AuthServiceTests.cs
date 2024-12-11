using DockerHubBackend.Dto.Request;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace DockerHubBackend.Tests.UnitTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPasswordHasher<string>> _mockPasswordHasher;
        private readonly Mock<IJwtHelper> _mockJwtHelper;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<string>>();
            _mockJwtHelper = new Mock<IJwtHelper>();
            _service = new AuthService(_mockUserRepository.Object, _mockJwtHelper.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsLoginResponseWithJwt()
        {

            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "password123" };
            var user = new StandardUser { Id = Guid.NewGuid(), Email = credentials.Email, Password = "hashedPassword", LastPasswordChangeDate = DateTime.UtcNow, IsVerified = true };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(credentials.Email)).ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(It.IsAny<string>(), user.Password, credentials.Password))
                      .Returns(PasswordVerificationResult.Success);


            _mockJwtHelper.Setup(jwt => jwt.GenerateToken(user.GetType().Name, user.Id.ToString(), user.LastPasswordChangeDate))
                         .Returns("dummyToken");


            var result = await _service.Login(credentials);

            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Response.UserEmail);
            Assert.Equal(user.Id.ToString(), result.Response.UserId);
            Assert.Equal(user.GetType().Name, result.Response.UserRole);
            Assert.Equal("dummyToken", result.Jwt);
        }

        [Fact]
        public async Task Login_UserNotFound_ThrowsBadRequestException()
        {
            var credentials = new LoginCredentialsDto { Email = "notfound@example.com", Password = "password123" };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(credentials.Email)).ReturnsAsync((BaseUser)null);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.Login(credentials));
            Assert.Equal("Wrong email or password", exception.Message);
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsBadRequestException()
        {
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "wrongPassword" };
            var user = new StandardUser { Id = Guid.NewGuid(), Email = credentials.Email, Password = "hashedPassword", LastPasswordChangeDate = DateTime.UtcNow, IsVerified = true };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(credentials.Email)).ReturnsAsync(user);

            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(It.IsAny<string>(), user.Password, credentials.Password))
                      .Returns(PasswordVerificationResult.Failed);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.Login(credentials));
            Assert.Equal("Wrong email or password", exception.Message);
        }

        [Fact]
        public async Task Login_UnverifiedUser_ThrowsBadRequestException()
        {
            var credentials = new LoginCredentialsDto { Email = "test@example.com", Password = "password123" };
            var user = new StandardUser { Id = Guid.NewGuid(), Email = credentials.Email, Password = "hashedPassword", LastPasswordChangeDate = DateTime.UtcNow, IsVerified = false };

            _mockUserRepository.Setup(repo => repo.GetUserByEmail(credentials.Email)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(It.IsAny<string>(), user.Password, credentials.Password))
                        .Returns(PasswordVerificationResult.Success);
            _mockJwtHelper.Setup(jwt => jwt.GenerateToken(user.GetType().Name, user.Id.ToString(), user.LastPasswordChangeDate))
                         .Returns("dummyToken");

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.Login(credentials));
            Assert.Equal("Account not verified", exception.Message);
        }

    }
}
