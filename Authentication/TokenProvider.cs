using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Eatspress.Models;

namespace Eatspress.Authentication
{
    public class TokenProvider
    {
        private readonly TokenGenerator _gen;
        private readonly TokenValidator _val;

        public TokenProvider(TokenConfiguration cfg)
        {
            _gen = new TokenGenerator(cfg);
            _val = new TokenValidator(cfg);
        }

        public (string AccessToken, string RefreshToken) GenerateTokens(User u)
        {
            

            var access = _gen.CreateAccessToken(u);
            var refresh = _gen.CreateRefreshToken(u.User_Id);

            return (access, refresh);
        }

        public void Store(HttpResponse response, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, 
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        public string? Refresh(string refreshToken)
        {
            var principal = _val.Validate(refreshToken);
            if (principal == null) return null;
            if (principal.Claims.FirstOrDefault(c => c.Type == "typ")?.Value != "refresh") return null;

            var uid = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (uid == null) return null;

            // carry over claims except "typ"
            var claims = principal.Claims.Where(c => c.Type != "typ");
            return _gen.CreateAccessToken(
                new User{ 
                    User_Id = int.Parse(uid), 
                    Email = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value ?? "",
                    Role = new UserRole{ Role_Title = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User" }
                }
            );
        }
        public void Delete(HttpRequest request, HttpResponse response)
        {
            if(request.Cookies.ContainsKey("refreshToken"))
                response.Cookies.Delete("refreshToken");
        }
    }
}
