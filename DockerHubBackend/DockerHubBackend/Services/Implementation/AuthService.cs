﻿using DockerHubBackend.Dto.Request;
using DockerHubBackend.Dto.Response;
using DockerHubBackend.Exceptions;
using DockerHubBackend.Models;
using DockerHubBackend.Repository.Interface;
using DockerHubBackend.Security;
using DockerHubBackend.Services.Interface;
using DockerHubBackend.Startup;
using Microsoft.AspNetCore.Identity;

namespace DockerHubBackend.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;        
        private readonly IPasswordHasher<string> _passwordHasher;
        private readonly IJwtHelper _jwtHelper;
        private readonly IVerificationTokenRepository _verificationTokenRepository;
        private readonly IRandomTokenGenerator _randomTokenGenerator;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IJwtHelper jwtHelper, IPasswordHasher<string> passwordHasher, IVerificationTokenRepository verificationTokenRepository, IRandomTokenGenerator randomTokenGenerator, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _passwordHasher = passwordHasher;
            _verificationTokenRepository = verificationTokenRepository;
            _randomTokenGenerator = randomTokenGenerator;
            _logger = logger;
        }

        private async Task UpdateVerificationToken(SuperAdmin user, string token)
        {

            if (user.VerificationToken != null)
            {
                user.VerificationToken.Token = token;
                user.VerificationToken.CreatedAt = DateTime.UtcNow;
                user.VerificationToken.ValidUntil = DateTime.UtcNow.AddHours(1);
                await _verificationTokenRepository.Update(user.VerificationToken);
            }
            else
            {
                var verificationToken = new VerificationToken {
                    Token = token,
                    User = user,
                    UserId = user.Id,
                    ValidUntil = DateTime.UtcNow.AddHours(1)
                };
                await _verificationTokenRepository.Create(verificationToken);
            }
        }

        public async Task<LoginResponse> Login(LoginCredentialsDto credentials)
        {
            _logger.LogInformation("Pokušaj prijave korisnika sa email adresom: {Email}", credentials.Email);

            var user = await _userRepository.GetUserWithTokenByEmail(credentials.Email);
            if (user == null)
            {
                _logger.LogWarning("Neuspešna prijava: Korisnik sa email adresom {Email} nije pronađen.", credentials.Email);
                throw new UnauthorizedException("Wrong email or password");
            }
            if (_passwordHasher.VerifyHashedPassword(String.Empty, user.Password, credentials.Password) != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Neuspešna prijava: Korisnik sa email adresom {Email} nije pronađen.", credentials.Email);
                throw new UnauthorizedException("Wrong email or password");
            }
            if(user.GetType() == typeof(SuperAdmin) && !((SuperAdmin)user).IsVerified)
            {
                var verificationToken = _randomTokenGenerator.GenerateVerificationToken(256);
                await UpdateVerificationToken((SuperAdmin)user, verificationToken);
                _logger.LogWarning("Nalog nije verifikovan za korisnika {Email}. Generisan je token za verifikaciju.", credentials.Email);
                throw new AccountVerificationRequiredException("Account verification required", verificationToken);
            }
            var token = _jwtHelper.GenerateToken(user.GetType().Name, user.Id.ToString(), user.Email);

            _logger.LogInformation("Korisnik {Email} je uspešno prijavljen.", credentials.Email);

            LoginResponse response = new LoginResponse {
                AccessToken = token
            };
            return response;
        }
    }
}
