using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace NSI.Infra.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _key;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = Encoding.UTF8.GetBytes(
            _configuration["JwtSettings:Key"]!
        );
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(_key),

                ClockSkew = TimeSpan.Zero // penting
            };
        }

        public string GenerateToken(string userId, out DateTime expiredAt)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userId)

            };

            int expiryMinutes = 60; // default 60 menit
            var expiredMinutesStr = jwtSettings["ExpiredMinutes"];
            if (!string.IsNullOrEmpty(expiredMinutesStr))
            {
                expiryMinutes = int.Parse(expiredMinutesStr);
            }

            expiredAt = DateTime.UtcNow.AddMinutes(expiryMinutes);


            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expiredAt,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
