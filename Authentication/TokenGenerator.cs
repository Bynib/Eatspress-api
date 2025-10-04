using Eatspress.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eatspress.Authentication
{
    public class TokenGenerator
    {
        private readonly TokenConfiguration _cfg;

        public TokenGenerator(TokenConfiguration cfg)
        {
            _cfg = cfg;
        }

        public string CreateAccessToken(User u)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_cfg.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.User_Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, u.Email),
                new Claim(ClaimTypes.Role, u.Role?.Role_Title ?? "User")
            };

            var token = new JwtSecurityToken(
                issuer: _cfg.Issuer,
                audience: _cfg.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_cfg.ExpireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateRefreshToken(int userId)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_cfg.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("typ","refresh")
            };

            var token = new JwtSecurityToken(
                issuer: _cfg.Issuer,
                audience: _cfg.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
