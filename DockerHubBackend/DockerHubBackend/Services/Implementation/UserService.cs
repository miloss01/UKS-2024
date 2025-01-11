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

        public async Task<BaseUserDTO> RegisterStandardUser(RegisterUserDto registerUserDto)
        {
            var user = await RegisterUserAsync<StandardUser>(registerUserDto);
            return new BaseUserDTO(user);
        }

        public async Task<BaseUserDTO> RegisterAdmin(RegisterUserDto registerUserDto)
        {
            var user = await RegisterUserAsync<Admin>(registerUserDto);
            return new BaseUserDTO(user);
        }

        public List<StandardUser> GetAllStandardUsers()
        {
            return _userRepository.GetAllStandardUsers();
        }

        public void ChangeUserBadge(Badge badge, Guid userId)
        {
            _userRepository.ChangeUserBadge(badge, userId);
        }
        private async Task<BaseUser> RegisterUserAsync<TUser>(RegisterUserDto registerUserDto) where TUser : BaseUser
        {
            if (await _userRepository.GetUserByEmail(registerUserDto.Email) != null)
            {
                throw new BadRequestException("An account with the given email already exists.");
            }

            if (await _userRepository.GetUserByUsername(registerUserDto.Username) != null)
            {
                throw new BadRequestException("The given username is already in use.");
            }

            var hashedPassword = _passwordHasher.HashPassword(string.Empty, registerUserDto.Password);

            var user = (TUser)Activator.CreateInstance(typeof(TUser),
                registerUserDto.Email,
                registerUserDto.Username,
                hashedPassword,
                registerUserDto.Location);

            return await _userRepository.Create(user);
        }
    }
}
