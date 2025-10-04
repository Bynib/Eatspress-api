using Microsoft.Extensions.Configuration;

namespace Eatspress.Authentication
{
    public class TokenConfiguration
    {
        public string Key { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public int ExpireMinutes { get; }

        public TokenConfiguration(IConfiguration cfg)
        {
            Key = cfg["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key missing");
            Issuer = cfg["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer missing");
            Audience = cfg["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience missing");
            ExpireMinutes = int.TryParse(cfg["Jwt:ExpireMinutes"], out var m) ? m : 60;
        }
    }
}
