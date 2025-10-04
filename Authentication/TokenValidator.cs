using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eatspress.Authentication
{
    public class TokenValidator
    {
        private readonly TokenConfiguration _cfg;

        public TokenValidator(TokenConfiguration cfg)
        {
            _cfg = cfg;
        }

        public ClaimsPrincipal? Validate(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.Key));

                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _cfg.Issuer,
                    ValidAudience = _cfg.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
