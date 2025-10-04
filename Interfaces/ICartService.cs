using Eatspress.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourApp.ServiceModels;

namespace Eatspress.Interfaces
{
    public interface ICartService
    {
        Task<CartResponse> AddToCartAsync(CartRequest req);
        Task<IEnumerable<CartResponse>> GetUserCartAsync(int userId);
        Task<CartResponse?> UpdateCartAsync(CartRequest req);
        Task<bool> RemoveFromCartAsync(int userId, int itemId);
        Task<bool> RemoveAllFromCartAsync(int userId);
    }
}
