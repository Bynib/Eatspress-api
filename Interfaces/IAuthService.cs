using System.Threading.Tasks;
using Eatspress.ServiceModels;

namespace Eatspress.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResponse> RegisterAsync(RegistrationRequest req, HttpResponse res);
        Task<AuthenticationResponse> LoginAsync(LoginRequest req, HttpResponse res);
        Task<AuthenticationResponse?> GoogleAsync(string email, string password, HttpResponse res);
        string Refresh(string refreshToken);

        void Logout(HttpRequest req, HttpResponse res);
    }
}
