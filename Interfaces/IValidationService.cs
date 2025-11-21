using System.IO;
using System.Threading.Tasks;

namespace Eatspress.Interfaces
{
    public interface IValidationService
    {
        Task<bool> IsValidEmailAsync(string e);
        Task<bool> IsValidPhoneAsync(string p);
        Task<bool> IsValidImageAsync(IFormFile f);
    }
}
