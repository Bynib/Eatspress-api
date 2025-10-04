using Eatspress.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Interfaces
{
    public interface IOrderService
    {
        Task<int> PlaceOrderAsync(OrderRequest req);
        Task<OrderResponse?> GetByIdAsync(int id);
        Task<IEnumerable<OrderResponse>> GetByUserAsync(int userId);
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<bool> UpdateStatusAsync(int orderId, int statusId);
        Task<bool> CancelAsync(int orderId);
    }
}
