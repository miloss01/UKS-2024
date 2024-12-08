﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DockerHubBackend.Security
{
    public class JwtHelper
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly int _expiration; // Expiration of jwt in minutes

        public JwtHelper(IConfiguration configuration)
        {
            _key = configuration["Jwt:Key"];
            _issuer = configuration["Jwt:Issuer"];
            _expiration = Convert.ToInt32(configuration["Jwt:Expiration"]);
        }
        public string GenerateToken(string role, string userId, DateTime? lastPasswordChange)
        {
            string lastPasswordChangeStr = lastPasswordChange != null ? lastPasswordChange.ToString() : DateTime.MinValue.ToString("o");
            var claims = new[]
            {                
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim("LastPasswordChange", lastPasswordChangeStr)
            };

            var keyBytes = Encoding.UTF8.GetBytes(_key);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiration),
                Issuer = _issuer,
                Audience = _issuer,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
