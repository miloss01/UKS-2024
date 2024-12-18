using DockerHubBackend.Dto.Request;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace DockerHubBackend.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationTokenRepository _verificationTokenRepository;
        private readonly IPasswordHasher<string> _passwordHasher;

        public UserService(IUserRepository userRepository, IVerificationTokenRepository verificationTokenRepository, IPasswordHasher<string> passwordHasher)
        {
            _userRepository = userRepository;
            _verificationTokenRepository = verificationTokenRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var token = await _verificationTokenRepository.GetTokenByValue(changePasswordDto.Token);
            if (token == null || token.ValidUntil < DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid token");
            }
            token.User.LastPasswordChangeDate = DateTime.UtcNow;
            token.User.Password = _passwordHasher.HashPassword(String.Empty, changePasswordDto.NewPassword);
            ((SuperAdmin)token.User).IsVerified = true;
            await _userRepository.Update(token.User);
        }
    }
}
