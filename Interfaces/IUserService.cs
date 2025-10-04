// Interfaces/IUserService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Eatspress.ServiceModels;

namespace Eatspress.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> UpdateUserAsync(UpdateUserRequest request);
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse> GetByIdAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
