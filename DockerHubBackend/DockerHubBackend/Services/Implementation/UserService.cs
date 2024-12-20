using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
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

        public async Task<StandardUserDto> RegisterStandardUser(RegisterUserDto registerUserDto)
        {
            if(await _userRepository.GetUserByEmail(registerUserDto.Email) != null)
            {
                throw new BadRequestException("An account with given email aready exists");
            }
            if(await _userRepository.GetUserByUsername(registerUserDto.Username) != null)
            {
                throw new BadRequestException("A given username is already in use");
            }
            var hashedPassword = _passwordHasher.HashPassword(String.Empty, registerUserDto.Password);
            var user = new StandardUser
            {
                Email = registerUserDto.Email,
                Username = registerUserDto.Username,
                Password = hashedPassword,
                Location = registerUserDto.Location,
                Badge = Badge.NoBadge
            };
            StandardUser savedUser = (StandardUser) await _userRepository.Create(user);

            return new StandardUserDto(savedUser);
        }
    }
}
