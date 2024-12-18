using DockerHubBackend.Dto.Request;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DockerHubBackend.Tests.UnitTests
{

    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IVerificationTokenRepository> _mockVerificationTokenRepository;
        private readonly Mock<IPasswordHasher<string>> _mockPasswordHasher;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockVerificationTokenRepository = new Mock<IVerificationTokenRepository>();
            _mockPasswordHasher = new Mock<IPasswordHasher<string>>();
            _service = new UserService(_mockUserRepository.Object, _mockVerificationTokenRepository.Object, _mockPasswordHasher.Object);
        }

        [Fact]
        public async Task ChangePassword_NonExistingTokenThrowsUnauthorizedException()
        {
            var changePasswordDto = new ChangePasswordDto { NewPassword = "password", Token = "token" };

            _mockVerificationTokenRepository.Setup(repo => repo.GetTokenByValue(It.IsAny<string>())).ReturnsAsync((VerificationToken?)null);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.ChangePassword(changePasswordDto));
            Assert.Equal("Invalid token", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ExpiredTokenThrowsUnauthorizedException()
        {
            var changePasswordDto = new ChangePasswordDto { NewPassword = "password", Token = "token" };
            var user = new SuperAdmin { Id = Guid.NewGuid(), Email = "email@email.com", Password = "hashedPassword", LastPasswordChangeDate = DateTime.UtcNow, IsVerified = false };
            var verificationToken = new VerificationToken { Token = "token", ValidUntil = DateTime.UtcNow.AddHours(-1), User = user, UserId = user.Id};

            _mockVerificationTokenRepository.Setup(repo => repo.GetTokenByValue(It.IsAny<string>())).ReturnsAsync(verificationToken);

            var exception = await Assert.ThrowsAsync<UnauthorizedException>(() => _service.ChangePassword(changePasswordDto));
            Assert.Equal("Invalid token", exception.Message);
        }

        [Fact]
        public async Task ChangePassword_ValidTokenUpdatesUser()
        {
            var changePasswordDto = new ChangePasswordDto { NewPassword = "newPassword", Token = "token" };
            var user = new SuperAdmin { Id = Guid.NewGuid(), Email = "email@email.com", Password = "hashedPassword", LastPasswordChangeDate = null, IsVerified = false };
            var verificationToken = new VerificationToken { Token = "token", ValidUntil = DateTime.UtcNow.AddHours(1), User = user, UserId = user.Id };

            _mockVerificationTokenRepository.Setup(repo => repo.GetTokenByValue("token")).ReturnsAsync(verificationToken);
            _mockPasswordHasher.Setup(hasher => hasher.HashPassword(It.IsAny<string>(), changePasswordDto.NewPassword))
                        .Returns("newHashedPassword");
            
            await _service.ChangePassword(changePasswordDto);

            Assert.Equal("newHashedPassword", user.Password);
            Assert.True(user.IsVerified);
            Assert.True(user.LastPasswordChangeDate.HasValue);
            _mockUserRepository.Verify(repo => repo.Update(It.IsAny<BaseUser>()), Times.Once);
        }
    }
}
