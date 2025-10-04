using Eatspress.ServiceModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eatspress.Interfaces
{
    public interface IMenuService
    {
        Task<FoodItemResponse> CreateAsync(FoodItemRequest req);
        Task<IEnumerable<FoodItemResponse>> GetAllAsync();
        Task<FoodItemResponse?> GetByIdAsync(int id);
        Task<FoodItemResponse?> UpdateAsync(int id, FoodItemRequest req);
        Task<bool> DeleteAsync(int id);
    }
}
